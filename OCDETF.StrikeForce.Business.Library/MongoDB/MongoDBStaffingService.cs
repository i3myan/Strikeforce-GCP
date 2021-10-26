using Microsoft.Extensions.Logging;
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
    public class MongoDBStaffingService : MongoDBBaseService, IStaffingService
    {
        private IQuarterReportService QtrReportService { get; set; }
        private IMongoCollection<Staffing> StaffingList { get; set; }

        public MongoDBStaffingService(IMongoClient serviceClient, IQuarterReportService qtrReportService, ILogger appLogger, AppConfiguration appConfig) : base(serviceClient,appLogger, appConfig)
        {

            this.QtrReportService = qtrReportService;
            MongoDatabase = MongoClient.GetDatabase(appConfig.MongoDBName);
            StaffingList = MongoDatabase.GetCollection<Staffing>(StrikeForceTables.Staffing.ToString());
        }

        public Staffing Add(Staffing newRecord, string userID)
        {
            newRecord.DateCreated = DateTime.Now;
            newRecord.DateUpdated = DateTime.Now;
            newRecord.CreatedUserID = userID;
            newRecord.UpdatedUserID = userID;
            newRecord.IsActive = true;
            newRecord.ID = string.Empty;
            //newRecord.ID = Guid.NewGuid().ToString();

            //newRecord.RowKey = newRecord.ID;
            //newRecord.PartitionKey = newRecord.StrikeForceID;


            StaffingList.InsertOne(newRecord);


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

            //Insert into Mongo DB
            foreach (Staffing aStaffItem in staffRecords)
            {
                
                aStaffItem.QuarterlyReportID = newReport.ID;
                aStaffItem.StrikeForceID = newReport.StrikeForceID;
                Add(aStaffItem, userID);

            }
            return staffRecords;
        }

        public Staffing Delete(Staffing updateRecord, string userID)
        {
            updateRecord.IsActive = false;
            return Update(updateRecord, userID);
        }

        public IList<Staffing> Get(string quarterlyReportID)
        {
            IList<Staffing> reportStaffings = StaffingList.Find(sel => sel.QuarterlyReportID == quarterlyReportID && sel.IsActive).ToList();
            
            return reportStaffings.OrderBy(sel => sel.Order).ToList();
        }

        public IList<Staffing> GetByYear(int fiscalYear)
        {
            IList<string> quarterlyReportIDs = this.QtrReportService.GetReports(string.Empty, true).Where(sel => sel.FiscalYear == fiscalYear).Select(sel => sel.ID).ToList<string>();
            

            return StaffingList.Find(sel => quarterlyReportIDs.Contains(sel.QuarterlyReportID)).ToList().OrderBy(sel => sel.Order).ToList();
            
        }

        public Staffing Update(Staffing newRecord, string userID)
        {
            newRecord.UpdatedUserID = userID;
            newRecord.DateUpdated = DateTime.Now;
            
            StaffingList.ReplaceOne(sel => sel.ID == newRecord.ID, newRecord);

            return newRecord;
        }

        public IList<Staffing> UpdateRecords(IList<Staffing> staffRecords, string userID)
        {
            foreach (Staffing aStaffItem in staffRecords)
            {
                Update(aStaffItem, userID);
            }

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
            IList<Staffing> reportStaffings = new List<Staffing>();
            var result = this.QtrReportService.GetReports(strikeForceID, false);
            var prevQtrReport = result.Where(sel => sel.FiscalYear == fiscalYear && sel.Quarter == Qtr).FirstOrDefault();

            if (prevQtrReport != null)
            {
                reportStaffings = StaffingList.Find(sel => sel.StrikeForceID == strikeForceID && sel.QuarterlyReportID == prevQtrReport.ID && sel.IsActive).ToList();                
            }

            return reportStaffings.OrderBy(sel => sel.Order).ToList();
        }
    }
}
