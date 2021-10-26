using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.MongoDB
{
    public class MongoDBQuarterReportService : MongoDBBaseService, IQuarterReportService
    {
        private readonly IMongoCollection<QuarterlyReport> quarterlyReports;

        public MongoDBQuarterReportService(ILogger logger, AppConfiguration appConfig, IMongoClient mongoClient):base(mongoClient, logger, appConfig)
        {
            MongoDatabase = mongoClient.GetDatabase(appConfig.MongoDBName);
            quarterlyReports = MongoDatabase.GetCollection<QuarterlyReport>(StrikeForceTables.QuarterlyReports.ToString());
        }

        public QuarterlyReport Create(QuarterlyReport newReport, string userID)
        {
            newReport.DateCreated = DateTime.Now;
            newReport.DateUpdated = DateTime.Now;
            newReport.CreatedUserID = userID;
            newReport.UpdatedUserID = userID; 
            //newReport.ID = new ObjectId().ToString();
            newReport.IsActive = true;

            newReport.RowKey = newReport.ID;
            newReport.PartitionKey = newReport.StrikeForceID;
            var prevQuarterReport = GetPreviousQtrReport(newReport.StrikeForceID);
            if (prevQuarterReport != null)
            {
                newReport.Mission = prevQuarterReport.Mission;
                newReport.Structure = prevQuarterReport.Structure;
            }
            
            quarterlyReports.InsertOne(newReport);

            return newReport;

           
        }

        public IList<QuarterlyReport> Delete(QuarterlyReport deleteReport, string userID)
        {
            deleteReport.IsActive = false;
            Update(deleteReport, userID);

            return GetReports(deleteReport.StrikeForceID, false);
        }

        public QuarterlyReport GetReport(string quarterlyReportID, string strikeForceID, bool isAdmin)
        {
            QuarterlyReport result;
            if (!isAdmin)
                result = quarterlyReports.Find<QuarterlyReport>(sel => sel.ID == quarterlyReportID && sel.StrikeForceID == strikeForceID && sel.IsActive).FirstOrDefault();                
            else
                result = quarterlyReports.Find<QuarterlyReport>(sel => sel.ID == quarterlyReportID && sel.IsActive).FirstOrDefault();

            return result;
        }

        public IList<QuarterlyReport> GetReports(string strikeForceID, bool isAdmin)
        {
            IList<QuarterlyReport> result;

            if (!isAdmin)
                result = quarterlyReports.Find<QuarterlyReport>(sel => sel.StrikeForceID == strikeForceID && sel.IsActive).ToList();
            else
                result = quarterlyReports.Find<QuarterlyReport>(sel => sel.IsActive).ToList();

            return result;
        }

        public IList<QuarterlyReport> GetReportsByYear(int fiscalYear, string strikeForceID)
        {            
            IList<QuarterlyReport> result = quarterlyReports.Find<QuarterlyReport>(sel => sel.StrikeForceID == strikeForceID && sel.FiscalYear == fiscalYear && sel.IsActive).ToList();

            return result;
        }

        public QuarterlyReport Update(QuarterlyReport newReport, string userID)
        {
            newReport.DateUpdated = DateTime.Now;
            newReport.UpdatedUserID = userID;

            newReport.RowKey = newReport.ID;
            newReport.PartitionKey = newReport.StrikeForceID;

            quarterlyReports.ReplaceOne(sel => sel.ID == newReport.ID, newReport);

            return newReport;
        }

        public IList<ValidationResult> Validate(string quaterlyReportID)
        {
            throw new NotImplementedException();
        }


        private QuarterlyReport GetPreviousQtrReport(string strikeForceID)
        {
            var reports = this.GetReports(strikeForceID, false);
            var result = reports.OrderByDescending(sel => sel.FiscalYear).ThenBy(sel => sel.Quarter).FirstOrDefault();
            return result;
        }
    }
}
