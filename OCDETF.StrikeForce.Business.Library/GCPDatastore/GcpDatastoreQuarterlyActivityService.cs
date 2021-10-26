
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Core.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using Google.Cloud.Datastore.V1;

namespace OCDETF.StrikeForce.Business.Library
{
    public class GcpDatastoreQuarterlyActivityService : GcpDatastoreBaseService, IQuarterlyActivityService
    {
        protected IQuarterReportService QtrReportService { get; set; }
        protected IList<QuarterlyActivity> QuarterlyActivities { get; set; }

        public GcpDatastoreQuarterlyActivityService(DatastoreDb db, ILogger logger, AppConfiguration appConfig, IQuarterReportService qtrReportService) : base(db, appConfig, logger)
        {
            this.datastoreDb = db;          
            this.QtrReportService = qtrReportService;    
        }

        public QuarterlyActivity Add(QuarterlyActivity newRecord, string userID, string table)
        {
            newRecord.DateCreated = DateTime.UtcNow;
            newRecord.DateUpdated = DateTime.UtcNow;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;

            Entity qaEntity = newRecord.ToQuarterlyActivityEntity(this.datastoreDb, table);
            this.datastoreDb.Insert(qaEntity);
            
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
            List<Entity> entities = new List<Entity>();
            foreach (QuarterlyActivity qItem in activityData)
            {
                qItem.QuarterlyReportID = newReport.ID;
                qItem.StrikeForceID = newReport.StrikeForceID;
                qItem.IsActive = true;
                qItem.IsCommon = true;
                qItem.Category = table.ToString();
                qItem.CreatedUserID = userID;
                qItem.DateCreated = DateTime.UtcNow;
                qItem.UpdatedUserID = userID;
                qItem.DateUpdated = DateTime.UtcNow;


                entities.Add(qItem.ToQuarterlyActivityEntity(this.datastoreDb, table.ToString()));
            }
            this.datastoreDb.Insert(entities);

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
            Query query = new Query(table)
            {
                Filter = Filter.And(Filter.Equal("IsActive", true), Filter.Equal("QuarterlyReportID", quarterlyReportID))
            };

            IList<QuarterlyActivity> activityList = this.datastoreDb.RunQuery(query).Entities.Select(e => e.ToQuarterlyActivity()).OrderBy(sel => sel.Order).ToList();
            return activityList;
        }

        public IList<QuarterlyActivity> Get(string table)
        {
            IList<QuarterlyActivity> activityList = new List<QuarterlyActivity>();

            Query query = new Query(table)
            {
                Filter = Filter.Equal("IsActive", true),
            };

            activityList = this.datastoreDb.RunQuery(query).Entities.Select(e => e.ToQuarterlyActivity()).OrderBy(sel => sel.Order).ToList();
            return activityList;      
        }

        public IList<QuarterlyActivity> Update(QuarterlyActivity newMeasure, string userID, string table)
        {

            newMeasure.UpdatedUserID = userID;
            newMeasure.DateUpdated = DateTime.UtcNow;
            this.datastoreDb.Update(newMeasure.ToQuarterlyActivityEntity(this.datastoreDb, table));

            return Get(newMeasure.QuarterlyReportID, table);
        }

        public IList<QuarterlyActivity> UpdateRecords(IList<QuarterlyActivity> requiredData, string userID, string table)
        {
            var temp = requiredData.FirstOrDefault();
            if (temp == null)
                return new List<QuarterlyActivity>();

            foreach (QuarterlyActivity aItem in requiredData)
            {
                Update(aItem, userID, table);
            }

            return Get(temp.QuarterlyReportID, table);
        }

        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            throw new NotImplementedException();
        }


        private IList<QuarterlyActivity> GetActivityByQtr(string strikeForceID, int fiscalYear, int Qtr, string tableName)
        {
            QuarterlyActivities = this.Get(tableName);
            IList<QuarterlyActivity> reportStaffings = new List<QuarterlyActivity>();
            var result = this.QtrReportService.GetReports(strikeForceID, false);
            var prevQtrReport = result.Where(sel => sel.FiscalYear == fiscalYear && sel.Quarter == Qtr).FirstOrDefault();

            if (prevQtrReport != null)
            {
                reportStaffings = QuarterlyActivities.Where(sel => sel.StrikeForceID == strikeForceID && sel.QuarterlyReportID == prevQtrReport.ID && sel.Category == tableName).ToList();
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
