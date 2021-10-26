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
    public class AzTableQuarterlyActivityService : AzTableBaseService, IQuarterlyActivityService
    {
        private IQuarterReportService QtrReportService { get; set; }


        public AzTableQuarterlyActivityService() : base() { }

        public AzTableQuarterlyActivityService(TableServiceClient serviceClient, IQuarterReportService qtrReportService, AppConfiguration appConfig, ILogger logger) : base(serviceClient, appConfig, logger)
        {
            this.QtrReportService = qtrReportService;
        }

        public QuarterlyActivity Add(QuarterlyActivity newRecord, string userID, string table)
        {
            this.AzureTableService.CreateTableIfNotExists(table);
            TableClient = this.AzureTableService.GetTableClient(table.ToString());
            newRecord.DateCreated = DateTime.Now;
            newRecord.DateUpdated = DateTime.Now;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;
            newRecord.ID = Guid.NewGuid().ToString();

            newRecord.RowKey = newRecord.ID;
            newRecord.PartitionKey = newRecord.StrikeForceID;

            TableClient.AddEntity<QuarterlyActivity>(newRecord);
            return newRecord;
        }

        public IList<QuarterlyActivity> AddNewRecords(IList<QuarterlyActivity> activityData, QuarterlyReport newReport, string userID, StrikeForceTables table)
        {
            IList<QuarterlyActivity> previousActivity = new List<QuarterlyActivity>();
            if (newReport.Quarter > 1)
            {
                previousActivity = GetActivityByQtr(newReport.StrikeForceID, newReport.FiscalYear, newReport.Quarter - 1, table.ToString());
                if (previousActivity.Count == 0)
                    previousActivity = LoadFromConfiguration(table);
            }
            else
            {
                //Load default records from configuration  
                previousActivity = LoadFromConfiguration(table);
                
            }

            activityData = previousActivity.Where(sel => sel.StrikeForceID == newReport.StrikeForceID || string.IsNullOrEmpty(sel.StrikeForceID)).OrderBy(order => order.Order).ToList();
            foreach (QuarterlyActivity aItem in activityData)
            {
                aItem.QuarterlyReportID = newReport.ID;
                aItem.StrikeForceID = newReport.StrikeForceID;
            }

            //insert into azure table.
            this.AzureTableService.CreateTableIfNotExists(table.ToString());
            TableClient = this.AzureTableService.GetTableClient(table.ToString());
            foreach (QuarterlyActivity qItem in activityData)
            {
                qItem.ID = Guid.NewGuid().ToString();
                qItem.RowKey = qItem.ID;
                qItem.PartitionKey = qItem.StrikeForceID;

                qItem.CreatedUserID = userID;
                qItem.DateCreated = DateTime.Now;
                qItem.UpdatedUserID = userID;
                qItem.DateUpdated = DateTime.Now;
                qItem.IsActive = true;
                qItem.IsCommon = true;
                //qItem.FirstQuarter = "";
                //qItem.SecondQuarter = "";
                //qItem.ThirdQuarter = "";
                //qItem.FourthQuarter = "";
                qItem.Category = table.ToString();

                TableClient.AddEntity<QuarterlyActivity>(qItem);
            }

            return Get(newReport.ID, table.ToString());
        }

        private IList<QuarterlyActivity> LoadFromConfiguration(StrikeForceTables table)
        {
            IList<QuarterlyActivity> result = new List<QuarterlyActivity>();
            var temp = CloneUtility<AppConfiguration>.Clone(this.AppConfig);
            if (table == StrikeForceTables.OCDETF)
                result = temp.OCDETF;
            else if (table == StrikeForceTables.Required)
                result = temp.Required;
            else if (table == StrikeForceTables.Seizures)
                result = temp.Seizures;
            else if (table == StrikeForceTables.Specific)
                result = temp.Specific;

            return result;
        }

        public IList<QuarterlyActivity> Get(string quarterlyReportID, string table)
        {
            TableClient = this.AzureTableService.GetTableClient(table.ToString());
            IList<QuarterlyActivity> activityList = new List<QuarterlyActivity>();

            Pageable<QuarterlyActivity> queryResultsFilter = TableClient.Query<QuarterlyActivity>(filter: $"QuarterlyReportID eq '{quarterlyReportID}' and IsActive eq true");

            //foreach (QuarterlyActivity qEntity in queryResultsFilter)
            //{
            //    activityList.Add(qEntity);
            //}
            return queryResultsFilter.ToList().OrderBy(sel => sel.Order).ToList();
        }

        public IList<QuarterlyActivity> UpdateRecords(IList<QuarterlyActivity> requiredData, string userID, string table)
        {
            TableClient = this.AzureTableService.GetTableClient(table.ToString());
            foreach (QuarterlyActivity aItem in requiredData)
            {
                aItem.RowKey = aItem.ID;
                aItem.PartitionKey = aItem.StrikeForceID;
                aItem.UpdatedUserID = userID;
                aItem.DateUpdated = DateTime.Now;

                TableClient.UpsertEntity<QuarterlyActivity>(aItem);
            }

            var temp = requiredData.FirstOrDefault();
            string quarterlyReportID = string.Empty;
            if (temp != null)
                quarterlyReportID = temp.QuarterlyReportID;

            return Get(quarterlyReportID, table);
        }

        public IList<QuarterlyActivity> Update(QuarterlyActivity newMeasure, string userID, string table)
        {
            TableClient = this.AzureTableService.GetTableClient(table.ToString());

            newMeasure.UpdatedUserID = userID;
            newMeasure.DateUpdated = DateTime.Now;
            TableClient.UpsertEntity<QuarterlyActivity>(newMeasure);

            return Get(newMeasure.QuarterlyReportID, table);
        }

        public QuarterlyActivity Delete(QuarterlyActivity newMeasure, string userID, string table)
        {
            TableClient = this.AzureTableService.GetTableClient(table.ToString());

            newMeasure.UpdatedUserID = userID;
            newMeasure.DateUpdated = DateTime.Now;
            newMeasure.IsActive = false;
            TableClient.UpsertEntity<QuarterlyActivity>(newMeasure);

            return newMeasure;
        }



        private IList<QuarterlyActivity> GetActivityByQtr(string strikeForceID, int fiscalYear, int Qtr, string tableName)
        {
            IList<QuarterlyActivity> reportStaffings = new List<QuarterlyActivity>();
            var result = this.QtrReportService.GetReports(strikeForceID, false);
            var prevQtrReport = result.Where(sel => sel.FiscalYear == fiscalYear && sel.Quarter == Qtr).FirstOrDefault();

            if (prevQtrReport != null)
            {
                TableClient = this.AzureTableService.GetTableClient(tableName.ToString());
                Pageable<QuarterlyActivity> queryResultsFilter = TableClient.Query<QuarterlyActivity>(filter: $"StrikeForceID eq '{strikeForceID}' and QuarterlyReportID eq '{prevQtrReport.ID}' and Category eq '{tableName}'");

                reportStaffings = queryResultsFilter.ToList();
                //foreach (QuarterlyActivity qEntity in queryResultsFilter)
                //{
                //    reportStaffings.Add(qEntity);
                //}
            }

            return reportStaffings.OrderBy(sel => sel.Order).ToList();
        }

        public IList<QuarterlyActivity> Get(string table)
        {
            Pageable<QuarterlyActivity> queryResultsFilter;
            IList<QuarterlyActivity> result = new List<QuarterlyActivity>();
            TableClient = this.AzureTableService.GetTableClient(table.ToString());

            queryResultsFilter = TableClient.Query<QuarterlyActivity>(filter: $"Category eq '{table}'");

            //foreach (QuarterlyActivity qEntity in queryResultsFilter)
            //{
            //    result.Add(qEntity);
            //}

            return queryResultsFilter.ToList().OrderBy(sel => sel.Order).ToList();
        }

        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            IList<ValidationResult> result = new List<ValidationResult>();
            string[] tables = { StrikeForceTables.Required.ToString(), StrikeForceTables.Specific.ToString(), StrikeForceTables.Required.ToString() };

            foreach (string table in tables)
            {
                TableClient = this.AzureTableService.GetTableClient(table.ToString());
                Pageable<QuarterlyActivity> queryResultsFilter = TableClient.Query<QuarterlyActivity>(filter: $"QuarterlyReportID eq '{quarterlyReportID}' and Category eq '{table}'");
                QuarterlyReport aReport = this.QtrReportService.GetReport(quarterlyReportID, string.Empty, true);

                if (queryResultsFilter.Any())
                {
                    IList<QuarterlyActivity> validate = queryResultsFilter.ToList();
                    foreach (QuarterlyActivity aItem in validate)
                    {
                        if ((aReport.Quarter == 4 && string.IsNullOrEmpty(aItem.FourthQuarter))
                            || (aReport.Quarter == 3 && string.IsNullOrEmpty(aItem.ThirdQuarter))
                            || (aReport.Quarter == 2 && string.IsNullOrEmpty(aItem.SecondQuarter))
                            || (aReport.Quarter == 1 && string.IsNullOrEmpty(aItem.FirstQuarter)))
                            result.Add(new ValidationResult() { Category = table.Replace("x", string.Empty), Message = "\"" + aItem.ActivityName + "\" is Required!" });
                    }
                }
            }
            return result;
        }
    }
}
