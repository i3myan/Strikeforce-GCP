using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Google.Api.Gax;
using Google.Cloud.Datastore.V1;
using MongoDB.Driver;
using OCDETF.StrikeForce.Business.Library.Models;

namespace OCDETF.StrikeForce.Business.Library
{
    public class GcpDatastoreQuarterReportService : GcpDatastoreBaseService, IQuarterReportService
    {
        //protected KeyFactory keyFactory { get; set; }
        //protected IList<QuarterlyReport> quarterlyReports;

        public GcpDatastoreQuarterReportService(ILogger logger, AppConfiguration appConfig, DatastoreDb db) : base(db, appConfig, logger)
        {
            this.datastoreDb = db;
            //this.keyFactory = this.datastoreDb.CreateKeyFactory(StrikeForceTables.QuarterlyReports.ToString());
            //this.quarterlyReports = this.GetReports("", true);
        }

        public QuarterlyReport Create(QuarterlyReport newReport, string userID)
        {
            newReport.DateCreated = DateTime.UtcNow;
            newReport.DateUpdated = DateTime.UtcNow;
            newReport.CreatedUserID = userID;
            newReport.UpdatedUserID = userID;
            newReport.IsActive = true;

            var prevQuarterReport = GetPreviousQtrReport(newReport.StrikeForceID);
            if (prevQuarterReport != null)
            {
                newReport.Mission = prevQuarterReport.Mission;
                newReport.Structure = prevQuarterReport.Structure;
            }

            Entity newReportEntity = newReport.ToQuarterlyReportEntity(this.datastoreDb);          
            this.datastoreDb.Insert(newReportEntity);
            return newReportEntity.ToQuarterlyReport();
        }

        public IList<QuarterlyReport> Delete(QuarterlyReport deleteReport, string userID)
        {
            deleteReport.IsActive = false;
            Update(deleteReport, userID);

            return GetReports(deleteReport.StrikeForceID, false);
        }

        public QuarterlyReport GetReport(string quarterlyReportID, string strikeForceID, bool isAdmin)
        {
            return this.GetReports(strikeForceID, isAdmin).Where(r => r.ID == quarterlyReportID).FirstOrDefault();                  
        }


        public IList<QuarterlyReport> GetReports(string strikeForceID, bool isAdmin)
        {
            if(String.IsNullOrEmpty(strikeForceID))
            {
                return new List<QuarterlyReport>();
            }
            Query query;

            if (!isAdmin)
            {
                query = new Query(StrikeForceTables.QuarterlyReports.ToString())
                {
                    Filter = Filter.And(Filter.Equal("StrikeForceID", strikeForceID), Filter.Equal("IsActive", true))
                };
            }
            else
            {
                query = new Query(StrikeForceTables.QuarterlyReports.ToString())
                {
                    Filter = Filter.Equal("IsActive", true)
                };
            }

            IEnumerable<Entity> entities = this.datastoreDb.RunQuery(query).Entities;
            if (entities != null)
            {
                List<QuarterlyReport> reports = this.datastoreDb.RunQuery(query).Entities.Select(e => e.ToQuarterlyReport()).OrderBy(e => e.Name).ToList();
                return reports;
            }
            else
            {
                return new List<QuarterlyReport>();
            }
        }

        public IList<QuarterlyReport> GetReportsByYear(int fiscalYear, string strikeForceID)
        {
            return this.GetReports(strikeForceID, false).Where(r => r.FiscalYear == fiscalYear).ToList();    
        }

        public QuarterlyReport Update(QuarterlyReport newReport, string userID)
        {
            newReport.DateUpdated = DateTime.UtcNow;
            newReport.UpdatedUserID = userID;

            this.datastoreDb.Update(newReport.ToQuarterlyReportEntity(this.datastoreDb));
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
