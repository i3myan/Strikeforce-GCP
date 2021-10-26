using Azure;
using Azure.Data.Tables;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace OCDETF.StrikeForce.Business.Library.AzureTable
{
    public class AzTableQuarterReportService : AzTableBaseService, IQuarterReportService
    {
        public AzTableQuarterReportService():base() { }


        public AzTableQuarterReportService(TableServiceClient serviceClient, ILogger logger):base(serviceClient, logger)
        {
            
            AzureTableService.CreateTableIfNotExists(StrikeForceTables.QuarterlyReports.ToString());
            TableClient = AzureTableService.GetTableClient(StrikeForceTables.QuarterlyReports.ToString());
        }

        public QuarterlyReport Create(QuarterlyReport newReport, string userID)
        {
            newReport.CreatedUserID = userID;
            newReport.UpdatedUserID = userID;
            newReport.DateCreated = DateTime.Now;
            newReport.DateUpdated = DateTime.Now;
            newReport.Status = "Initiated";
            newReport.IsActive = true;
            newReport.ID = Guid.NewGuid().ToString();

            newReport.RowKey = newReport.ID;
            newReport.PartitionKey = newReport.StrikeForceID;
            var prevQuarterReport = GetPreviousQtrReport(newReport.StrikeForceID);
            if (prevQuarterReport != null)
            {
                newReport.Mission = prevQuarterReport.Mission;
                newReport.Structure = prevQuarterReport.Structure;               
            }
            
            TableClient.AddEntity<QuarterlyReport>(newReport);

            return newReport;
        }

        public IList<QuarterlyReport> Delete(QuarterlyReport deleteReport, string userID)
        {

            deleteReport.DateUpdated = DateTime.Now;
            deleteReport.UpdatedUserID = userID;
            deleteReport.IsActive = false;
            TableClient.UpsertEntity<QuarterlyReport>(deleteReport);

            return GetReports(deleteReport.StrikeForceID, true);
        }

        /// <summary>
        /// Returns a Quarterly report if IsActive is true for given criteria, includes not non active for admin.
        /// </summary>
        /// <param name="quarterlyReportID">Quarterly Report ID</param>
        /// <param name="strikeForceID">StrikeForceID</param>
        /// <param name="isAdmin">Admin user?</param>
        /// <returns></returns>
        public QuarterlyReport GetReport(string quarterlyReportID, string strikeForceID, bool isAdmin)
        {            
            Pageable<QuarterlyReport> queryResultsFilter;

            if (!isAdmin)
                queryResultsFilter = TableClient.Query<QuarterlyReport>(filter: $"ID eq '{quarterlyReportID}' and IsActive eq true and StrikeForceID eq '{strikeForceID}' ");
            else
                queryResultsFilter = TableClient.Query<QuarterlyReport>(filter: $"ID eq '{quarterlyReportID}' and IsActive eq true");
            
            return queryResultsFilter.FirstOrDefault();
        }

        /// <summary>
        /// Returns a Quarterly report(s) for all strikeforces if admin, else based on strikeforceID.
        /// </summary>
        /// <param name="strikeForceID"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public IList<QuarterlyReport> GetReports(string strikeForceID, bool isAdmin)
        {
            Pageable<QuarterlyReport> queryResultsFilter;

            if (!isAdmin && !string.IsNullOrEmpty(strikeForceID))
                queryResultsFilter = TableClient.Query<QuarterlyReport>(filter: $"StrikeForceID eq '{strikeForceID}' and IsActive eq true");
            else 
                queryResultsFilter = TableClient.Query<QuarterlyReport>(filter: $"IsActive eq true");
           
            return queryResultsFilter.ToList();
        }

        public IList<QuarterlyReport> GetReportsByYear(int fiscalYear,string strikeForceID)
        {
            Pageable<QuarterlyReport> queryResultsFilter;

            queryResultsFilter = TableClient.Query<QuarterlyReport>(filter: $"FiscalYear eq {fiscalYear} and StrikeForceID eq '{strikeForceID}' and IsActive eq true");


            return queryResultsFilter.ToList();
        }

        /// <summary>
        /// Upserts the given quarterly report.
        /// </summary>
        /// <param name="newReport"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public QuarterlyReport Update(QuarterlyReport newReport, string userID)
        {
            newReport.UpdatedUserID = userID;
            newReport.DateUpdated = DateTime.Now;

            newReport.RowKey = newReport.ID;
            newReport.PartitionKey = newReport.StrikeForceID;

            TableClient.UpsertEntity<QuarterlyReport>(newReport);

            return newReport;
        }

        /// <summary>
        /// Validates quarterly Report required fields.
        /// </summary>
        /// <param name="quarterlyReportID"></param>
        /// <returns></returns>
        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            IList<ValidationResult> result = new List<ValidationResult>();
            Pageable<QuarterlyReport> queryResultsFilter;
            queryResultsFilter = TableClient.Query<QuarterlyReport>(filter: $"ID eq '{quarterlyReportID}'");
            
            foreach (QuarterlyReport aReport in queryResultsFilter)
            {
                if (aReport.Structure == null || (aReport.Mission != null  || aReport.Mission == null && aReport.Mission.Trim().Length ==0))
                    result.Add(new ValidationResult() { Category = "Mission", Message = "Mission is Required!"});
                if (aReport.Structure == null || (aReport.Structure != null && aReport.Structure.Trim().Length == 0))
                    result.Add(new ValidationResult() { Category = "Structure", Message = "Structure is Required!" });
                if (aReport.HeadsUp == null || (aReport.HeadsUp != null && aReport.HeadsUp.Trim().Length == 0))
                    result.Add(new ValidationResult() { Category = "HeadsUp", Message = "Heads Up is Required!" });
                if (aReport.Challenges == null || (aReport.Challenges != null && aReport.Challenges.Trim().Length == 0))
                    result.Add(new ValidationResult() { Category = "Challenges", Message = "Challenge(s) is Required!" });
                if (aReport.OfficeLocations == null || (aReport.OfficeLocations != null && aReport.OfficeLocations.Trim().Length == 0))
                    result.Add(new ValidationResult() { Category = "Office Locations", Message = "Office Location(s) is Required!" });
            }
            return result;
        }

        /// <summary>
        /// Returns latest quarterly report for given strike force ID
        /// </summary>
        /// <param name="strikeForceID">StrikeForceID</param>
        /// <returns>QuarterlyReport object</returns>
        private QuarterlyReport GetPreviousQtrReport(string strikeForceID)
        {
            var reports = this.GetReports(strikeForceID, false);
            var result = reports.OrderByDescending(sel => sel.FiscalYear).ThenBy(sel => sel.Quarter).FirstOrDefault();
            return result;
        }

        
    }
}
