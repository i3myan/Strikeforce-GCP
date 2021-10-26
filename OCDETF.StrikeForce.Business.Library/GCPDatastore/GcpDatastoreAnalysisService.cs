using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Core.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using Google.Cloud.Datastore.V1;
using OCDETF.StrikeForce.Business.Library.Models.Lookups;

namespace OCDETF.StrikeForce.Business.Library
{
    public class GcpDatastoreAnalysisService : GcpDatastoreBaseService, IQtrActivityAnalysisService
    {
        private IQuarterlyActivityService QuarterlyActivityService { get; set; }
        private IQuarterReportService QuarterReportService { get; set; }
        private IStaffingService StaffService { get; set; }
        private ILookupService LookupService { get; set; }

        public GcpDatastoreAnalysisService() { }

        public GcpDatastoreAnalysisService(IQuarterlyActivityService quarterlyActivityService, IStaffingService staffService, ILookupService lookupService, IQuarterReportService quarterReportService)
        {
            this.QuarterlyActivityService = quarterlyActivityService;
            this.StaffService = staffService;
            this.LookupService = lookupService;
            this.QuarterReportService = quarterReportService;
        }

        public IList<SummaryAnalysis> Total(string table, int fiscalYear, int[] quarters)
        {

            IList<SummaryAnalysis> result;

            if (table == StrikeForceTables.Staffing.ToString())
                result = GetStaffing(table, fiscalYear, quarters);
            else
                result = GetQuarterlyActivity(table, fiscalYear, quarters);

            foreach (SummaryAnalysis aItem in result)
            {
                aItem.Total = Convert.ToInt32(string.IsNullOrEmpty(aItem.Atlanta) ? "0" : aItem.Atlanta) + Convert.ToInt32(string.IsNullOrEmpty(aItem.Baltimore) ? "0" : aItem.Baltimore) + Convert.ToInt32(string.IsNullOrEmpty(aItem.Boston) ? "0" : aItem.Boston)
                    + Convert.ToInt32(string.IsNullOrEmpty(aItem.Chicago) ? "0" : aItem.Chicago) + Convert.ToInt32(string.IsNullOrEmpty(aItem.Cleveland) ? "0" : aItem.Cleveland) + Convert.ToInt32(string.IsNullOrEmpty(aItem.Dallas) ? "0" : aItem.Dallas)
                    + Convert.ToInt32(string.IsNullOrEmpty(aItem.Denver) ? "0" : aItem.Denver) + Convert.ToInt32(string.IsNullOrEmpty(aItem.Detroit) ? "0" : aItem.Detroit) + Convert.ToInt32(string.IsNullOrEmpty(aItem.ElPaso) ? "0" : aItem.ElPaso)
                    + Convert.ToInt32(string.IsNullOrEmpty(aItem.Houston) ? "0" : aItem.Houston) + Convert.ToInt32(string.IsNullOrEmpty(aItem.KansasCity) ? "0" : aItem.KansasCity) + Convert.ToInt32(string.IsNullOrEmpty(aItem.LosAngeles) ? "0" : aItem.LosAngeles)
                    + Convert.ToInt32(string.IsNullOrEmpty(aItem.NewYork) ? "0" : aItem.NewYork) + Convert.ToInt32(string.IsNullOrEmpty(aItem.Phoenix) ? "0" : aItem.Phoenix) + Convert.ToInt32(string.IsNullOrEmpty(aItem.Sacramento) ? "0" : aItem.Sacramento)
                    + Convert.ToInt32(string.IsNullOrEmpty(aItem.SanDiego) ? "0" : aItem.SanDiego) + Convert.ToInt32(string.IsNullOrEmpty(aItem.SanJuan) ? "0" : aItem.SanJuan) + Convert.ToInt32(string.IsNullOrEmpty(aItem.StLouis) ? "0" : aItem.StLouis)
                    + Convert.ToInt32(string.IsNullOrEmpty(aItem.Tampa) ? "0" : aItem.Tampa);
            }
            return result;
        }

        private IList<SummaryAnalysis> GetQuarterlyActivity(string table, int fiscalYear, int[] quarters)
        {
            // Get all force locations
            IList<StrikeForceNames> forces = this.LookupService.GetForceLocations();

            // Get all records for specific measure table
            IList<QuarterlyActivity> result = this.QuarterlyActivityService.Get(table);

            // Get all reports
            IList<QuarterlyReport> qtrReports = this.QuarterReportService.GetReports(string.Empty, true);

            int quarter = quarters.Max();

            //Find the latest quarter report ids for given fiscal year
            var quarterlyReportIds = qtrReports.Where(filter => (filter.FiscalYear == fiscalYear && filter.Quarter == quarter && filter.IsActive == true) && filter.IsActive == true).GroupBy(group => new { group.StrikeForceID }).Select(sel => sel.OrderByDescending(x => x.Quarter).Select(sel => sel.ID).FirstOrDefault());

            int order = 0;
            var final = result
                .Where(sel => quarterlyReportIds.Contains(sel.QuarterlyReportID))
                .GroupBy(group => group.ActivityName)
                .Select(sel => new SummaryAnalysis
                {
                    ActivityName = sel.Key,
                    Order = ++order,
                    Atlanta = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Atlanta").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Baltimore = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Baltimore").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Boston = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Boston").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Chicago = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Chicago").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Cleveland = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Cleveland").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Dallas = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Dallas").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),

                    Denver = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Denver").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Detroit = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Detroit").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    ElPaso = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "El Paso").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Houston = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Houston").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    KansasCity = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Kansas City").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    LosAngeles = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Los Angeles").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),

                    NewYork = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "New York").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Phoenix = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Phoenix").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    Sacramento = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Sacramento").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    SanDiego = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "San Diego").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    SanJuan = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "San Juan").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                    StLouis = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "St. Louis").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),

                    Tampa = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Tampa").FirstOrDefault().ID).Select(sel => sel.Total).FirstOrDefault(),
                }).ToList<SummaryAnalysis>();


            return final;
        }
        private IList<SummaryAnalysis> GetStaffing(string table, int fiscalYear, int[] quarters)
        {
            int quarter = quarters.Max();

            // Get all force locations
            IList<StrikeForceNames> forces = this.LookupService.GetForceLocations();

            // Get all reports
            IList<QuarterlyReport> qtrReports = this.QuarterReportService.GetReports(string.Empty, true);

            //Get quarterlyreportids for fiscalyear and quarter.
            var quarterlyReportIds = qtrReports.Where(filter => (filter.FiscalYear == fiscalYear && filter.Quarter == quarter && filter.IsActive == true) && filter.IsActive == true).GroupBy(group => new { group.StrikeForceID }).Select(sel => sel.OrderByDescending(x => x.Quarter).Select(sel => sel.ID).FirstOrDefault());


            IList<Staffing> staffingRecords = this.StaffService.GetByYear(fiscalYear);
            int order = 0;
            var staff = staffingRecords
                .Where(sel => quarterlyReportIds.Contains(sel.QuarterlyReportID))
                .GroupBy(group => group.AgencyName)
                .Select(sel => new SummaryAnalysis
                {
                    ActivityName = sel.Key,
                    Order = ++order,
                    Atlanta = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Atlanta").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Baltimore = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Baltimore").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Boston = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Boston").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Chicago = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Chicago").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Cleveland = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Cleveland").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Dallas = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Dallas").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),

                    Denver = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Denver").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Detroit = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Detroit").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    ElPaso = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "El Paso").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Houston = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Houston").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    KansasCity = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Kansas City").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    LosAngeles = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Los Angeles").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),

                    NewYork = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "New York").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Phoenix = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Phoenix").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    Sacramento = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Sacramento").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    SanDiego = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "San Diego").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    SanJuan = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "San Juan").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                    StLouis = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "St. Louis").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),

                    Tampa = sel.Where(filter => filter.StrikeForceID == forces.Where(sel => sel.Name == "Tampa").FirstOrDefault().ID).Select(sel => sel.Total.ToString()).FirstOrDefault(),
                }).ToList<SummaryAnalysis>();

            return staff;
        }
    }
}
