using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.MongoDB
{
    public class MongoDBQuarterlyActivityService : MongoDBBaseService, IQuarterlyActivityService
    {
        private IMongoCollection<QuarterlyActivity> QuarterlyActivities { get; set; }
        private IQuarterReportService QtrReportService { get; set; }

        public MongoDBQuarterlyActivityService(ILogger logger, AppConfiguration appConfig, IMongoClient mongoClient, IQuarterReportService qtrReportService) : base(mongoClient, logger, appConfig)
        {
            MongoDatabase = mongoClient.GetDatabase(appConfig.MongoDBName);
            this.QtrReportService = qtrReportService;
        }

        public QuarterlyActivity Add(QuarterlyActivity newRecord, string userID, string table)
        {
            QuarterlyActivities = MongoDatabase.GetCollection<QuarterlyActivity>(table);
            
            newRecord.DateCreated = DateTime.Now;
            newRecord.DateUpdated = DateTime.Now;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;
            newRecord.ID = string.Empty;
            //newRecord.ID = new ObjectId(Guid.NewGuid().ToString()).ToString();

            newRecord.RowKey = newRecord.ID;
            newRecord.PartitionKey = newRecord.StrikeForceID;

            QuarterlyActivities.InsertOne(newRecord);
            return newRecord;
        }

        public IList<QuarterlyActivity> AddNewRecords(IList<QuarterlyActivity> activityData, QuarterlyReport newReport, string userID, StrikeForceTables table)
        {
            QuarterlyActivities = MongoDatabase.GetCollection<QuarterlyActivity>(table.ToString());

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
            
            foreach (QuarterlyActivity qItem in activityData)
            {

                qItem.IsActive = true;
                qItem.IsCommon = true;
                qItem.Category = table.ToString();

                Add(qItem, userID, table.ToString());
            }

            return Get(newReport.ID, table.ToString());
        }

        public QuarterlyActivity Delete(QuarterlyActivity newMeasure, string userID, string table)
        {
            
            newMeasure.IsActive = false;

            Update(newMeasure, userID, table);

            return newMeasure;
        }

        public IList<QuarterlyActivity> Get(string quarterlyReportID, string table)
        {            
            IList<QuarterlyActivity> activityList = new List<QuarterlyActivity>();
            QuarterlyActivities = MongoDatabase.GetCollection<QuarterlyActivity>(table);

            activityList = QuarterlyActivities.Find(sel => sel.QuarterlyReportID == quarterlyReportID && sel.Category == table && sel.IsActive).ToList();
            return activityList.OrderBy(sel => sel.Order).ToList();
        }

        public IList<QuarterlyActivity> Get(string table)
        {
            IList<QuarterlyActivity> activityList = new List<QuarterlyActivity>();
            QuarterlyActivities = MongoDatabase.GetCollection<QuarterlyActivity>(table);

            activityList = QuarterlyActivities.Find(sel => sel.Category == table && sel.IsActive).ToList();
            return activityList.OrderBy(sel => sel.Order).ToList();
        }

        public IList<QuarterlyActivity> Update(QuarterlyActivity newMeasure, string userID, string table)
        {
            QuarterlyActivities = MongoDatabase.GetCollection<QuarterlyActivity>(table);

            newMeasure.UpdatedUserID = userID;
            newMeasure.DateUpdated = DateTime.Now;
            QuarterlyActivities.ReplaceOne(sel => sel.ID == newMeasure.ID, newMeasure);

            return Get(newMeasure.QuarterlyReportID, table);
        }

        public IList<QuarterlyActivity> UpdateRecords(IList<QuarterlyActivity> requiredData, string userID, string table)
        {
            
            foreach (QuarterlyActivity aItem in requiredData)
            {                
                Update(aItem, userID, table);                
            }

            var temp = requiredData.FirstOrDefault();
            string quarterlyReportID = string.Empty;
            if (temp != null)
                quarterlyReportID = temp.QuarterlyReportID;

            return Get(quarterlyReportID, table);
        }

        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            throw new NotImplementedException();
        }


        private IList<QuarterlyActivity> GetActivityByQtr(string strikeForceID, int fiscalYear, int Qtr, string tableName)
        {
            IList<QuarterlyActivity> reportStaffings = new List<QuarterlyActivity>();
            var result = this.QtrReportService.GetReports(strikeForceID, false);
            var prevQtrReport = result.Where(sel => sel.FiscalYear == fiscalYear && sel.Quarter == Qtr).FirstOrDefault();

            if (prevQtrReport != null)
            {
                reportStaffings = QuarterlyActivities.Find(sel => sel.StrikeForceID == strikeForceID && sel.QuarterlyReportID == prevQtrReport.ID && sel.Category == tableName).ToList();                
            }

            return reportStaffings.OrderBy(sel => sel.Order).ToList();
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
    }
}
