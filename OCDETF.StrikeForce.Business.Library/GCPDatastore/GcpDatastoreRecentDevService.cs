using Google.Cloud.Datastore.V1;
using OCDETF.StrikeForce.Business.Library.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCDETF.StrikeForce.Core.Library;
using System.Collections;
using MongoDB.Driver;

namespace OCDETF.StrikeForce.Business.Library
{
    public class GcpDatastoreRecentDevService :GcpDatastoreBaseService, IRecentDevelopmentService
    {
        protected IList<RecentDevelopments> recentDevelopments;
        public GcpDatastoreRecentDevService() { }

        public GcpDatastoreRecentDevService(DatastoreDb db, AppConfiguration appConfig, ILogger logger) : base(db, appConfig, logger)
        {
            this.datastoreDb = db;      
        }

        public IList<RecentDevelopments> GetRecentDevelopments()
        {
            Query query = new Query(StrikeForceTables.RecentDevelopments.ToString())
            {
                Filter = Filter.Equal("IsActive", true)          
            };

            List<RecentDevelopments> results = this.datastoreDb.RunQuery(query).Entities.Select(d => d.ToRecentDevelopments()).OrderBy(d => d.StrikeForceID).ToList();
            return results;
        }

        public IList<RecentDevelopments> Add(RecentDevelopments newRecord, string userID)
        {
            newRecord.DateCreated = DateTime.UtcNow;
            newRecord.DateUpdated = DateTime.UtcNow;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;
            this.datastoreDb.Insert(newRecord.ToRecentDevelopmentsEntity(this.datastoreDb));
            //this.recentDevelopments = this.GetRecentDevelopments();
            return Get(newRecord.QuarterlyReportID);
        }

        public IList<RecentDevelopments> AddRecords(IList<RecentDevelopments> recentRecords, QuarterlyReport newReport, string userID)
        {
            List<Entity> entities = new List<Entity>();
            foreach (RecentDevelopments aRecentItem in recentRecords)
            {
                aRecentItem.ID = Guid.NewGuid().ToString();
                aRecentItem.StrikeForceID = newReport.StrikeForceID;
                aRecentItem.QuarterlyReportID = newReport.ID;
                aRecentItem.CreatedUserID = userID;
                aRecentItem.DateCreated = DateTime.UtcNow;
                aRecentItem.UpdatedUserID = userID;
                aRecentItem.DateUpdated = DateTime.UtcNow;
                aRecentItem.IsActive = true;
                entities.Add(aRecentItem.ToRecentDevelopmentsEntity(this.datastoreDb));
            }
            this.datastoreDb.Insert(entities);
            return entities.Select(e => e.ToRecentDevelopments()).ToList();
        }

        public IList<RecentDevelopments> Delete(RecentDevelopments updateRecord, string userID)
        {
            updateRecord.IsActive = false;
            return Update(updateRecord, userID);
        }

        public IList<RecentDevelopments> Get(string quarterlyReportID)
        {           
            return this.GetRecentDevelopments().Where(sel => sel.QuarterlyReportID == quarterlyReportID).OrderBy(sel => sel.Order).ToList();
        }

        public IList<RecentDevelopments> Update(RecentDevelopments newRecord, string userID)
        {
            newRecord.UpdatedUserID = userID;
            newRecord.DateUpdated = DateTime.UtcNow;
            this.datastoreDb.Update(newRecord.ToRecentDevelopmentsEntity(this.datastoreDb));

            return Get(newRecord.QuarterlyReportID);
        }

        public IList<RecentDevelopments> UpdateRecords(IList<RecentDevelopments> recentRecords, QuarterlyReport newReport, string userID)
        {
            List<Entity> entities = new List<Entity>();
            foreach (RecentDevelopments aRecentItem in recentRecords)
            {
                aRecentItem.StrikeForceID = newReport.StrikeForceID;
                aRecentItem.QuarterlyReportID = newReport.ID;
                aRecentItem.UpdatedUserID = userID;
                aRecentItem.DateUpdated = DateTime.UtcNow;

                entities.Add(aRecentItem.ToRecentDevelopmentsEntity(this.datastoreDb));
            }
            this.datastoreDb.Update(entities);
            return Get(newReport.ID);
        }

        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            throw new NotImplementedException();
        }
    }
}
