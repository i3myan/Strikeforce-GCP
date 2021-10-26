using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCDETF.StrikeForce.Business.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Business.Library.PDF;
using System;
using System.Collections.Generic;
using System.IO;

namespace OCDETF.StrikeForce.Web.API.Controllers
{
    [Route("api/analysis")]
    [ApiController]
    public class QuarterlyActivityAnalysisController : ControllerBase
    {
        private IQtrActivityAnalysisService analysisService { get; set; }

        private IQuarterlyActivityService quarterlyActivityService { get; set; }
        IStaffingService staffService { get; set; }
        IQuarterReportService quarterReportService { get; set;}

        private AppConfiguration appConfig { get; set; }

        public QuarterlyActivityAnalysisController(IQtrActivityAnalysisService analysisService, IQuarterlyActivityService quarterlyActivityService, IStaffingService staffService, IQuarterReportService quarterReportService, AppConfiguration appConfig)
        {
            this.analysisService = analysisService;
            this.quarterlyActivityService = quarterlyActivityService;
            this.staffService = staffService;
            this.quarterReportService = quarterReportService;
            this.appConfig = appConfig;
        }

        [Authorize(Roles = "Administrator")]
        [Route("get-summary/{table}/{fiscalYear}/{fiscalQuarter}")]
        [HttpGet]
        public IList<SummaryAnalysis> Get(string table, int fiscalYear, int fiscalQuarter)
        {
            return this.analysisService.Total(table, fiscalYear, new int[] { fiscalQuarter });
        }

        //[Authorize(Roles = "Administrator")]
        [Route("get-pdf/{fiscalYear}/{fiscalQuarter}")]
        [HttpGet]
        public FileStreamResult Get(int fiscalYear, int fiscalQuarter)
        {

            byte[] result =  new SummaryPDFService(this.analysisService).CreateSummaryAnalysis(fiscalYear, new int[] { fiscalQuarter });
            return new FileStreamResult(new MemoryStream(result), "application/pdf;charset=UTF-8")
            {
                FileDownloadName = "Analysis-" + Guid.NewGuid().ToString() + ".pdf"
            };
        }

        [AllowAnonymous]
        [Route("get-pdf/report/{quarterlyReportID}")]
        [HttpGet]
        public void PrintPDF(string quarterlyReportID)
        {
            new QtrReportPDFService(this.quarterReportService, this.quarterlyActivityService, this.staffService).CreatePDF(quarterlyReportID);
            
        }
    }
}
