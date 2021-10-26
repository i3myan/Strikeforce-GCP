using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCDETF.StrikeForce.Business.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Business.Library.Models.Lookups;
using OCDETF.StrikeForce.Core.Library;
using OCDETF.StrikeForce.Web.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Controllers
{
    [Route("api/quarterly")]
    [ApiController]
    public class QuarterlyController : ControllerBase
    {
        private IQuarterReportService quarterReportService { get; set; }
        private ILookupService lookupService { get; set; }
        private IStaffingService staffService { get; set; }
        private IUserService userService { get; set; }
        private IQuarterlyActivityService activityService { get; set; }
        private AppConfiguration appConfig { get; set; }
        private IMapper myAutoMapper { get; set; }

        public QuarterlyController(IQuarterReportService reportService, IQuarterlyActivityService activityService, IUserService userService, ILookupService lookupService, IStaffingService staffService, AppConfiguration appConfig, IMapper mapper)
        {
            this.quarterReportService = reportService;
            this.activityService = activityService;
            this.lookupService = lookupService;
            this.staffService = staffService;
            this.userService = userService;
            this.appConfig = appConfig;
            this.myAutoMapper = mapper;
        }


        //[Authorize(Roles = "Administrator, Owner")]
        [Route("create-new")]
        [HttpPost]
        public xQuarterlyReport Add(xQuarterlyReport newReport)
        {

            QuarterlyReport aReport = new QuarterlyReport();
            myAutoMapper.Map(newReport, aReport);

            IList<StrikeForceNames> forceNames = this.lookupService.GetForceLocations();
            var temp = forceNames.Where(sel => sel.ID == aReport.StrikeForceID).FirstOrDefault();
            if (temp != null)
                aReport.StrikeForceName = temp.Name;
            

            var result = quarterReportService.Create(aReport, HttpContext.User.Identity.Name);
            myAutoMapper.Map(result, newReport);
                        

            this.staffService.AddRecords(newReport.StaffingData, result, HttpContext.User.Identity.Name);
            this.activityService.AddNewRecords(newReport.OCDETFCases, result, HttpContext.User.Identity.Name, StrikeForceTables.OCDETF);
            this.activityService.AddNewRecords(newReport.RequiredData, result, HttpContext.User.Identity.Name, StrikeForceTables.Required);
            this.activityService.AddNewRecords(newReport.SeizuresData, result, HttpContext.User.Identity.Name, StrikeForceTables.Seizures);
            this.activityService.AddNewRecords(newReport.SpecificData, result, HttpContext.User.Identity.Name, StrikeForceTables.Specific);
            return newReport;
        }


        //[Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("update-report")]
        [HttpPost]
        public xQuarterlyReport Update(xQuarterlyReport newReport)
        {

            QuarterlyReport aReport = new QuarterlyReport();
            myAutoMapper.Map(newReport, aReport);
            var result = quarterReportService.Update(aReport, HttpContext.User.Identity.Name);
            myAutoMapper.Map(result, newReport);

            return newReport;
        }


        //[Authorize(Roles = "Administrator")]
        [Route("delete-report")]
        [HttpPost]
        public IList<QuarterlyReport> Delete(QuarterlyReport deleteReport)
        {

            return this.quarterReportService.Delete(deleteReport, HttpContext.User.Identity.Name);
        }

        //[Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("recent/{strikeForceID}")]
        [HttpGet]
        public IList<xQuarterlyReport> Recent(string strikeForceID)
        {
            bool isAdmin = HttpContext.User.IsInRole("Administrator");

            IList<xQuarterlyReport> result = new List<xQuarterlyReport>();
            var myReports = this.quarterReportService.GetReports(strikeForceID, isAdmin);

            myReports = myReports.OrderByDescending(sel => sel.DateUpdated).Take(10).ToList();

            this.myAutoMapper.Map(myReports, result);            

            return result;
        }


        //[Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("get-reports/{strikeForceID}")]
        [HttpGet]
        public IList<xQuarterlyReport> GetReports(string strikeForceID)
        {
            bool isAdmin = HttpContext.User.IsInRole("Administrator");
            
            IList<xQuarterlyReport> result = new List<xQuarterlyReport>();
            var myReports = this.quarterReportService.GetReports(strikeForceID, isAdmin);
            myReports = myReports.OrderByDescending(sel => sel.DateUpdated).ToList();
            this.myAutoMapper.Map(myReports, result);
          
            foreach (xQuarterlyReport aReport in result)
            {
                aReport.StaffingData = this.staffService.Get(aReport.ID);
                aReport.SeizuresData = this.activityService.Get(aReport.ID, StrikeForceTables.Seizures.ToString());
                aReport.SpecificData = this.activityService.Get(aReport.ID, StrikeForceTables.Specific.ToString());
                aReport.OCDETFCases = this.activityService.Get(aReport.ID, StrikeForceTables.OCDETF.ToString());
                aReport.RequiredData = this.activityService.Get(aReport.ID, StrikeForceTables.Required.ToString());
            }

            return result;
        }

        //[Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("get-report/{quarterlyReportID}")]
        [HttpGet]
        public xQuarterlyReport GetReport(string quarterlyReportID)
        {
            string strikeForceID = string.Empty;
            bool isAdmin = HttpContext.User.IsInRole("Administrator");
            User currentUser = this.userService.Get(HttpContext.User.Identity.Name);
            if (currentUser != null)
                strikeForceID = currentUser.StrikeForceID;

            xQuarterlyReport result = new xQuarterlyReport();
            var myReports = this.quarterReportService.GetReport(quarterlyReportID, strikeForceID, isAdmin);
            
            this.myAutoMapper.Map(myReports, result);
            return result;
        }

        //[Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("validate/{quarterlyReportID}")]
        [HttpGet]
        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            IList<ValidationResult> result = this.quarterReportService.Validate(quarterlyReportID);
            return result;
        }

        //[Authorize(Roles = "Administrator")]
        [Route("get-year/{fiscalYear}/{strikeForceID}")]
        [HttpGet]
        public IList<QuarterlyReport> GetQuarterlyReportID(int fiscalYear, string strikeForceID)
        {
            var result = this.quarterReportService.GetReportsByYear(fiscalYear, strikeForceID);
            return result;
        }
    }
}
