using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library.AzureTable
{
    public class AzTableStaffingService : AzTableBaseService, IStaffingService
    {
        private IQuarterReportService QtrReportService { get; set; }

        public AzTableStaffingService(TableServiceClient serviceClient, IQuarterReportService qtrReportService, ILogger appLogger, AppConfiguration appConfig) : base(serviceClient, appConfig, appLogger)
        {

            this.QtrReportService = qtrReportService;
            AzureTableService.CreateTableIfNotExists(StrikeForceTables.Staffing.ToString());
            TableClient = AzureTableService.GetTableClient(StrikeForceTables.Staffing.ToString());
        }

        public Staffing Add(Staffing newRecord, string userID)
        {
            newRecord.DateCreated = DateTime.Now;
            newRecord.DateUpdated = DateTime.Now;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;

            newRecord.ID = Guid.NewGuid().ToString();

            newRecord.RowKey = newRecord.ID;
            newRecord.PartitionKey = newRecord.StrikeForceID;

            TableClient.AddEntity<Staffing>(newRecord);
            Pageable<Staffing> queryResultsFilter = TableClient.Query<Staffing>(filter: $"ID eq '{newRecord.ID}' ");

            foreach (Staffing qEntity in queryResultsFilter)
            {
                newRecord = qEntity;
            }
            return newRecord;
        }

        public IList<Staffing> AddRecords(IList<Staffing> staffRecords, QuarterlyReport newReport, string userID)
        {
            IList<Staffing> previousQtrStaffings = new List<Staffing>();
            if (newReport.Quarter > 1)
            {
                previousQtrStaffings = GetStaffingByQtr(newReport.StrikeForceID, newReport.FiscalYear, newReport.Quarter - 1);
            }
            if (previousQtrStaffings.Count > 0)
            {
                staffRecords = previousQtrStaffings;
            }
            else
            {
                //Load default records from configuration
                var temp = CloneUtility<AppConfiguration>.Clone(this.AppConfig);
                staffRecords = temp.Staffing.OrderBy(sel => sel.Order).ToList();
            }


            staffRecords.Where(sel => sel.StrikeForceID == newReport.StrikeForceID || string.IsNullOrEmpty(sel.StrikeForceID)).OrderBy(order => order.AgencyName).ToList();
            foreach (Staffing aStaff in staffRecords)
            {
                aStaff.QuarterlyReportID = newReport.ID;
                aStaff.StrikeForceID = newReport.StrikeForceID;
            }

            //Insert into Azure Table.
            foreach (Staffing aStaffItem in staffRecords)
            {
                aStaffItem.ID = Guid.NewGuid().ToString();
                aStaffItem.RowKey = aStaffItem.ID;
                aStaffItem.PartitionKey = aStaffItem.StrikeForceID;
                aStaffItem.CreatedUserID = userID;
                aStaffItem.DateCreated = DateTime.Now;
                aStaffItem.UpdatedUserID = userID;
                aStaffItem.DateUpdated = DateTime.Now;
                aStaffItem.IsActive = true;


                TableClient.AddEntity<Staffing>(aStaffItem);
            }
            return staffRecords;
        }

        public Staffing Update(Staffing newRecord, string userID)
        {
            throw new NotImplementedException();
        }

        public IList<Staffing> UpdateRecords(IList<Staffing> staffRecords, string userID)
        {

            foreach (Staffing aStaffItem in staffRecords)
            {
                aStaffItem.RowKey = aStaffItem.ID;
                aStaffItem.PartitionKey = aStaffItem.StrikeForceID;
                aStaffItem.UpdatedUserID = userID;
                aStaffItem.DateUpdated = DateTime.Now;

                TableClient.UpsertEntity<Staffing>(aStaffItem);
            }

            var temp = staffRecords.FirstOrDefault();
            string quarterlyReportID = string.Empty;
            if (temp != null)
                quarterlyReportID = temp.QuarterlyReportID;

            return Get(quarterlyReportID);
        }

        public Staffing Delete(Staffing staffItem, string userID)
        {

            staffItem.UpdatedUserID = userID;
            staffItem.DateUpdated = DateTime.Now;
            staffItem.IsActive = false;

            TableClient.UpsertEntity<Staffing>(staffItem);


            return staffItem;
        }


        public IList<Staffing> Get(string quarterlyReportID)
        {
            IList<Staffing> reportStaffings = new List<Staffing>();

            Pageable<Staffing> queryResultsFilter = TableClient.Query<Staffing>(filter: $"QuarterlyReportID eq '{quarterlyReportID}' and IsActive eq true");

            foreach (Staffing qEntity in queryResultsFilter)
            {
                reportStaffings.Add(qEntity);
            }
            return reportStaffings.OrderBy(sel => sel.Order).ToList();
        }

        private IList<Staffing> GetStaffingByQtr(string strikeForceID, int fiscalYear, int Qtr)
        {
            IList<Staffing> reportStaffings = new List<Staffing>();
            var result = this.QtrReportService.GetReports(strikeForceID, false);
            var prevQtrReport = result.Where(sel => sel.FiscalYear == fiscalYear && sel.Quarter == Qtr).FirstOrDefault();

            if (prevQtrReport != null)
            {
                Pageable<Staffing> queryResultsFilter = TableClient.Query<Staffing>(filter: $"StrikeForceID eq '{strikeForceID}' and QuarterlyReportID eq '{prevQtrReport.ID}'");

                foreach (Staffing qEntity in queryResultsFilter)
                {
                    reportStaffings.Add(qEntity);
                }
            }

            return reportStaffings.OrderBy(sel => sel.Order).ToList();
        }

        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            IList<ValidationResult> result = new List<ValidationResult>();

            TableClient = this.AzureTableService.GetTableClient(StrikeForceTables.Staffing.ToString());
            Pageable<Staffing> queryResultsFilter = TableClient.Query<Staffing>(filter: $"QuarterlyReportID eq '{quarterlyReportID}'");
            

            if (queryResultsFilter.Any())
            {
                IList<Staffing> validate = queryResultsFilter.ToList();
                foreach (Staffing aItem in validate)
                {
                    if (!aItem.NumberOfAgents.HasValue || !aItem.NumberOfAnalysts.HasValue || !aItem.NumberOfFederalTFOs.HasValue || !aItem.OtherNumbers.HasValue)
                        result.Add(new ValidationResult() { Category = "Staffing", Message = "\"" + aItem.AgencyName + "\" is Required!" });
                }
            }

            return result;
        }

        public IList<Staffing> GetByYear(int fiscalYear)
        {
            IList<string> quarterlyReportIDs = this.QtrReportService.GetReports(string.Empty, true).Where(sel => sel.FiscalYear == fiscalYear).Select(sel => sel.ID).ToList<string>();
            //StringBuilder filterString = new();
            StringBuilder filterString = new StringBuilder();
            IList<Staffing> result = new List<Staffing>();

            if (quarterlyReportIDs.Count>0)
            {
                foreach (string reportID in quarterlyReportIDs)
                {
                    filterString = filterString.Append(" or QuarterlyReportID eq '" + reportID + "'");
                }
                Pageable<Staffing> queryResultsFilter = TableClient.Query<Staffing>(filter: $"{filterString.ToString().Substring(4)}");
                result = queryResultsFilter.ToList();
            }
            return result.OrderBy(sel => sel.Order).ToList();
            
        }
    }
}
