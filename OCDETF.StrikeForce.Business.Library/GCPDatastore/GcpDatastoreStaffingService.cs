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

namespace OCDETF.StrikeForce.Business.Library
{
    public class GcpDatastoreStaffingService : GcpDatastoreBaseService, IStaffingService
    {
     
        protected IQuarterReportService QtrReportService { get; set; }
        protected IList<Staffing> StaffingList { get; set; }

        public GcpDatastoreStaffingService(ILogger logger, AppConfiguration appConfig, DatastoreDb db) : base(db, appConfig, logger)
        {
            this.datastoreDb = db;
            //this.StaffingList = this.GetStaffings();
        }

        //return all staffings
        public IList<Staffing> GetStaffings()
        {
            Query query = new Query(StrikeForceTables.Staffing.ToString())
            {
                Filter = Filter.Equal("IsActive", true),
            };
        
            IEnumerable<Entity> entities = this.datastoreDb.RunQuery(query).Entities;
            if(entities == null || !entities.Any())
            {
                return new List<Staffing> ();
            }
            else
            {
                List<Staffing> results = entities.Select(e => e.Tostaffing()).OrderBy(e => e.StrikeForceID).ToList();
                return results;
            }      
        }

        public Staffing Add(Staffing newRecord, string userID)
        {
            newRecord.DateCreated = DateTime.UtcNow;
            newRecord.DateUpdated = DateTime.UtcNow;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;

            Entity staffingEntity = newRecord.ToStaffingEntity(this.datastoreDb);
            this.datastoreDb.Insert(staffingEntity);         
            return staffingEntity.Tostaffing();
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

            List<Entity> entitiesToAdd = new List<Entity>();
            foreach (Staffing aStaffItem in staffRecords)
            {
                aStaffItem.QuarterlyReportID = newReport.ID;
                aStaffItem.StrikeForceID = newReport.StrikeForceID;
                aStaffItem.IsActive = true;
                entitiesToAdd.Add(aStaffItem.ToStaffingEntity(this.datastoreDb));
            }
            this.datastoreDb.Insert(entitiesToAdd);
            return entitiesToAdd.Select(e => e.Tostaffing()).ToList();
            
        }

        public Staffing Delete(Staffing updateRecord, string userID)
        {
            updateRecord.IsActive = false;
            return Update(updateRecord, userID);
        }

        public IList<Staffing> Get(string quarterlyReportID)
        {
            Query query = new Query(StrikeForceTables.Staffing.ToString())
            {
                Filter = Filter.And(Filter.Equal("QuarterlyReportID", quarterlyReportID), Filter.Equal("IsActive", true))
                         
            };

            IEnumerable<Entity> entities = this.datastoreDb.RunQuery(query).Entities;
            if (entities == null || !entities.Any())
            {
                return new List<Staffing>();
            }
            else
            {
                List<Staffing> results = entities.Select(e => e.Tostaffing()).OrderBy(e => e.Order).ToList();
                return results;
            }
        }

        public IList<Staffing> GetByYear(int fiscalYear)
        {
            this.StaffingList = this.GetStaffings();
            IList<string> quarterlyReportIDs = this.QtrReportService.GetReports(string.Empty, true).Where(sel => sel.FiscalYear == fiscalYear).Select(sel => sel.ID).ToList<string>();
            return StaffingList.Where(sel => quarterlyReportIDs.Contains(sel.QuarterlyReportID)).ToList().OrderBy(sel => sel.Order).ToList();
        }

        public Staffing Update(Staffing newRecord, string userID)
        {
            newRecord.UpdatedUserID = userID;
            newRecord.DateUpdated = DateTime.UtcNow;
            this.datastoreDb.Update(newRecord.ToStaffingEntity(this.datastoreDb));
            return newRecord;
        }

        public IList<Staffing> UpdateRecords(IList<Staffing> staffRecords, string userID)
        {
            List<Entity> entities = new List<Entity>();
            foreach (Staffing aStaffItem in staffRecords)
            {
                aStaffItem.UpdatedUserID = userID;
                aStaffItem.DateUpdated = DateTime.UtcNow;             
                entities.Add(aStaffItem.ToStaffingEntity(this.datastoreDb));
               
            }

            this.datastoreDb.Update(entities);

            var temp = staffRecords.FirstOrDefault();
            string quarterlyReportID = string.Empty;
            if (temp != null)
                quarterlyReportID = temp.QuarterlyReportID;

            return Get(quarterlyReportID);
        }

        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            throw new NotImplementedException();
        }

        private IList<Staffing> GetStaffingByQtr(string strikeForceID, int fiscalYear, int Qtr)
        {
            this.StaffingList = this.GetStaffings();
            IList<Staffing> reportStaffings = new List<Staffing>();
            var result = this.QtrReportService.GetReports(strikeForceID, false);
            var prevQtrReport = result.Where(sel => sel.FiscalYear == fiscalYear && sel.Quarter == Qtr).FirstOrDefault();

            if (prevQtrReport != null)
            {
                reportStaffings = StaffingList.Where(sel => sel.StrikeForceID == strikeForceID && sel.QuarterlyReportID == prevQtrReport.ID && sel.IsActive).ToList();
            }

            return reportStaffings.OrderBy(sel => sel.Order).ToList();
        }

    }
}
