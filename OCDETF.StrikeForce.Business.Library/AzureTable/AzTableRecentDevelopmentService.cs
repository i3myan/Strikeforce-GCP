using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.AzureTable
{
    public class AzTableRecentDevelopmentService : AzTableBaseService, IRecentDevelopmentService
    {
        
       
        public AzTableRecentDevelopmentService(TableServiceClient serviceClient, ILogger logger):base(serviceClient, logger)
        {            
            AzureTableService.CreateTableIfNotExists(StrikeForceTables.RecentDevelopments.ToString());
            TableClient = AzureTableService.GetTableClient(StrikeForceTables.RecentDevelopments.ToString());
        }

        public IList<RecentDevelopments> Get(string quarterlyReportID)
        {
            IList<RecentDevelopments> recents = new List<RecentDevelopments>();

            Pageable<RecentDevelopments> queryResultsFilter = TableClient.Query<RecentDevelopments>(filter: $"QuarterlyReportID eq '{quarterlyReportID}' and IsActive eq true");

            foreach (RecentDevelopments qEntity in queryResultsFilter)
            {
                recents.Add(qEntity);
            }
            return recents.OrderByDescending(sel => sel.Order).ToList();
        }

        public IList<RecentDevelopments> Add(RecentDevelopments newRecord, string userID)
        {
            newRecord.DateCreated = DateTime.Now;
            newRecord.DateUpdated = DateTime.Now;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;

            newRecord.ID = Guid.NewGuid().ToString();

            newRecord.RowKey = newRecord.ID;
            newRecord.PartitionKey = newRecord.StrikeForceID;

            TableClient.AddEntity<RecentDevelopments>(newRecord);

            return Get(newRecord.QuarterlyReportID);
        }

        public IList<RecentDevelopments> Update(RecentDevelopments newRecord, string userID)
        {
            

            newRecord.UpdatedUserID = userID;
            newRecord.DateUpdated = DateTime.Now;
            TableClient.UpsertEntity<RecentDevelopments>(newRecord);

            return Get(newRecord.QuarterlyReportID);
        }

        public IList<RecentDevelopments> Delete(RecentDevelopments updateRecord, string userID)
        {
            updateRecord.UpdatedUserID = userID;
            updateRecord.DateUpdated = DateTime.Now;
            updateRecord.IsActive = false;

            TableClient.UpsertEntity<RecentDevelopments>(updateRecord);


            return Get(updateRecord.QuarterlyReportID);
        }

        public IList<RecentDevelopments> AddRecords(IList<RecentDevelopments> recentRecords, QuarterlyReport newReport, string userID)
        {            
            //Insert into Azure Table.
            foreach (RecentDevelopments aRecentItem in recentRecords)
            {
                aRecentItem.ID = Guid.NewGuid().ToString();
                aRecentItem.StrikeForceID = newReport.StrikeForceID;
                aRecentItem.QuarterlyReportID = newReport.ID;

                aRecentItem.RowKey = aRecentItem.ID;
                aRecentItem.PartitionKey = aRecentItem.StrikeForceID;

                aRecentItem.CreatedUserID = userID;
                aRecentItem.DateCreated = DateTime.Now;
                aRecentItem.UpdatedUserID = userID;
                aRecentItem.DateUpdated = DateTime.Now;
                aRecentItem.IsActive = true;


                TableClient.AddEntity<RecentDevelopments>(aRecentItem);
            }
            return recentRecords;
        }

        public IList<RecentDevelopments> UpdateRecords(IList<RecentDevelopments> recentRecords, QuarterlyReport newReport, string userID)
        {
            foreach (RecentDevelopments aRecentItem in recentRecords)
            {
                aRecentItem.RowKey = aRecentItem.ID;
                aRecentItem.PartitionKey = aRecentItem.StrikeForceID;

                aRecentItem.UpdatedUserID = userID;
                aRecentItem.DateUpdated = DateTime.Now;

                TableClient.UpsertEntity<RecentDevelopments>(aRecentItem);
            }
            

            return Get(newReport.ID);
        }

        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            IList<ValidationResult> result = new List<ValidationResult>();
            Pageable<RecentDevelopments> queryResultsFilter;
            queryResultsFilter = TableClient.Query<RecentDevelopments>(filter: $"QuarterlyReportID eq '{quarterlyReportID}'");
           
            int count = queryResultsFilter.Count();
            
            if (count > 10)
            {
                result.Add(new ValidationResult() { Category = "Recent Developments", Message = "Limit to only upto 10 Record(s)" });
            }
            return result;
        }
    }
}
