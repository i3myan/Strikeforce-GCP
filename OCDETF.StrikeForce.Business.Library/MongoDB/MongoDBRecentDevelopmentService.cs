using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.MongoDB
{
    public class MongoDBRecentDevelopmentService : MongoDBBaseService, IRecentDevelopmentService
    {
        private readonly IMongoCollection<RecentDevelopments> recentDevelopments;
        public MongoDBRecentDevelopmentService() { }

        public MongoDBRecentDevelopmentService(IMongoClient mongoClient, AppConfiguration appConfig, ILogger logger):base(mongoClient, logger, appConfig) {

            MongoDatabase = mongoClient.GetDatabase(appConfig.MongoDBName);
            recentDevelopments = MongoDatabase.GetCollection<RecentDevelopments>(StrikeForceTables.RecentDevelopments.ToString());
        }

        public IList<RecentDevelopments> Add(RecentDevelopments newRecord, string userID)
        {
            newRecord.DateCreated = DateTime.Now;
            newRecord.DateUpdated = DateTime.Now;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;
            newRecord.ID = string.Empty;

            recentDevelopments.InsertOne(newRecord);

            return Get(newRecord.QuarterlyReportID);
        }

        public IList<RecentDevelopments> AddRecords(IList<RecentDevelopments> recentRecords, QuarterlyReport newReport, string userID)
        {
            foreach (RecentDevelopments aRecentItem in recentRecords)
            {
                aRecentItem.ID = Guid.NewGuid().ToString();
                aRecentItem.StrikeForceID = newReport.StrikeForceID;
                aRecentItem.QuarterlyReportID = newReport.ID;


                aRecentItem.CreatedUserID = userID;
                aRecentItem.DateCreated = DateTime.Now;
                aRecentItem.UpdatedUserID = userID;
                aRecentItem.DateUpdated = DateTime.Now;
                aRecentItem.IsActive = true;


                recentDevelopments.InsertOne(aRecentItem);
            }
            return recentRecords;
        }

        public IList<RecentDevelopments> Delete(RecentDevelopments updateRecord, string userID)
        {
            updateRecord.IsActive = false;
            return Update(updateRecord, userID);
        }

        public IList<RecentDevelopments> Get(string quarterlyReportID)
        {
            IList<RecentDevelopments> recents = new List<RecentDevelopments>();

            return recentDevelopments.Find(sel => sel.QuarterlyReportID == quarterlyReportID && sel.IsActive).ToList().OrderBy(sel => sel.Order).ToList();
            
        }

        public IList<RecentDevelopments> Update(RecentDevelopments newRecord, string userID)
        {
            newRecord.UpdatedUserID = userID;
            newRecord.DateUpdated = DateTime.Now;

            recentDevelopments.ReplaceOne(sel => sel.ID == newRecord.ID, newRecord);

            return Get(newRecord.QuarterlyReportID);
        }

        public IList<RecentDevelopments> UpdateRecords(IList<RecentDevelopments> recentRecords, QuarterlyReport newReport, string userID)
        {
            foreach (RecentDevelopments aRecentItem in recentRecords)
            {
                aRecentItem.StrikeForceID = newReport.StrikeForceID;
                aRecentItem.QuarterlyReportID = newReport.ID;

                Update(aRecentItem, userID);
            }
            return Get(newReport.ID);
        }

        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            throw new NotImplementedException();
        }
    }
}
