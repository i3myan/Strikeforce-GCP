using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCDETF.StrikeForce.Business.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Controllers
{
    [Route("api/quarterlyactvity")]
    [ApiController]
    public class QuarterlyActivityController : ControllerBase
    {
        private IQuarterlyActivityService ActivityService { get; set; }


        public QuarterlyActivityController(IQuarterlyActivityService activityService)
        {
            this.ActivityService = activityService;
        }


        /// <summary>
        /// Create a new specific measure item.
        /// </summary>
        /// <param name="newMeasure"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("new-specific-measure")]
        [HttpPost]
        public QuarterlyActivity AddSpecificMeasure(QuarterlyActivity newMeasure)
        {
            newMeasure.IsCommon = false;
            newMeasure.Category = StrikeForceTables.Specific.ToString();
            var result = this.ActivityService.Add(newMeasure, HttpContext.User.Identity.Name, StrikeForceTables.Specific.ToString());           
            return result;
        }

        /// <summary>
        /// Delete a specific measure item. soft delete.
        /// </summary>
        /// <param name="newMeasure"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("delete-specific-measure")]
        [HttpPost]
        public QuarterlyActivity DeleteSpecificMeasure(QuarterlyActivity newMeasure)
        {
            var result = this.ActivityService.Delete(newMeasure, HttpContext.User.Identity.Name, StrikeForceTables.Specific.ToString());
            return result;
        }


        /// <summary>
        /// Update a measure(seizure, required, specific) and  for any table. Uses Category property to determine table.
        /// </summary>
        /// <param name="updateMeasures"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("update-measure")]
        [HttpPost]
        public IList<QuarterlyActivity> UpdateSpecific(IList<QuarterlyActivity> updateMeasures)
        {
            string tableName = updateMeasures.FirstOrDefault().Category;
            var result = this.ActivityService.UpdateRecords(updateMeasures, HttpContext.User.Identity.Name, tableName);
            return result;
        }

        /// <summary>
        /// Create a new seizure item.
        /// </summary>
        /// <param name="newSeizure"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("new-seizure-measure")]
        [HttpPost]
        public QuarterlyActivity AddNewSeizure(QuarterlyActivity newSeizure)
        {
            newSeizure.IsCommon = false;
            newSeizure.Category = StrikeForceTables.Seizures.ToString();
            var result = this.ActivityService.Add(newSeizure, HttpContext.User.Identity.Name, StrikeForceTables.Seizures.ToString());
            return result;
        }

        /// <summary>
        /// Get Seizure Information list for given quarterly report ID.
        /// </summary>
        /// <param name="quarterlyReportID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("get-seizure/{quarterlyReportID}")]
        [HttpGet]
        public IList<QuarterlyActivity> GetSeizure(string quarterlyReportID)
        {
            
            var result = this.ActivityService.Get(quarterlyReportID, StrikeForceTables.Seizures.ToString());
            return result;
        }

        /// <summary>
        /// Get Specific Information list for given quarterly report ID.
        /// </summary>
        /// <param name="quarterlyReportID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("get-specific/{quarterlyReportID}")]
        [HttpGet]
        public IList<QuarterlyActivity> GetSpecific(string quarterlyReportID)
        {

            var result = this.ActivityService.Get(quarterlyReportID, StrikeForceTables.Specific.ToString());
            return result;
        }

        /// <summary>
        /// Get Required Information items for given quarterly report ID.
        /// </summary>
        /// <param name="quarterlyReportID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("get-required/{quarterlyReportID}")]
        [HttpGet]
        public IList<QuarterlyActivity> GetRequired(string quarterlyReportID)
        {

            var result = this.ActivityService.Get(quarterlyReportID, StrikeForceTables.Required.ToString());
            return result;
        }

        /// <summary>
        /// Get OCDETF Cases list for given quarterlyreport ID.
        /// </summary>
        /// <param name="quarterlyReportID">Quarterly Report ID</param>
        /// <returns> QuarterlyReport object</returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("get-ocdetf/{quarterlyReportID}")]
        [HttpGet]
        public IList<QuarterlyActivity> GetOCDETF(string quarterlyReportID)
        {

            var result = this.ActivityService.Get(quarterlyReportID, StrikeForceTables.OCDETF.ToString());
            return result;
        }

        /// <summary>
        /// Validate a given Quarterly Report.
        /// </summary>
        /// <param name="quarterlyReportID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("validate/{quarterlyReportID}")]
        [HttpGet]
        public IList<ValidationResult> Validate(string quarterlyReportID)
        {

            var result = this.ActivityService.Validate(quarterlyReportID);
            return result;
        }

        
    }
}
