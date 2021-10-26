using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Business.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Web.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Controllers
{
    [Route("api/staffing")]
    [ApiController]
    public class StaffingController : ControllerBase
    {        
        private readonly IStaffingService staffService;        
        private readonly ILogger logger;

        public StaffingController(IStaffingService staffService, ILogger<UsersController> applogger)
        {                        
            this.logger = applogger;
            this.staffService = staffService;
        }

        /// <summary>
        /// Update Staffing records
        /// </summary>
        /// <param name="staffingData"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner")]
        [Route("update-staff")]
        [HttpPost]
        public IList<Staffing> UpdateStaffing(IList<Staffing> staffingData)
        {
            this.logger.LogInformation("Updating Staffing Records!");
            var result = this.staffService.UpdateRecords(staffingData, HttpContext.User.Identity.Name);

            return result;
        }

        /// <summary>
        /// Add a new Staffing record.
        /// </summary>
        /// <param name="newMeasure"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("new-staff")]
        [HttpPost]
        public Staffing NewStaffing(Staffing newMeasure)
        {
            var result = this.staffService.Add(newMeasure, HttpContext.User.Identity.Name);
            return result;
        }

        /// <summary>
        /// Delete a Staffing Record.
        /// </summary>
        /// <param name="newMeasure"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("delete-staff")]
        [HttpPost]
        public Staffing DeleteSpecificMeasure(Staffing newMeasure)
        {
            var result = this.staffService.Delete(newMeasure, HttpContext.User.Identity.Name);
            return result;
        }

        /// <summary>
        /// Get Staffing records for given quarterly report ID. 
        /// </summary>
        /// <param name="quarterlyReportID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("get-staff/{quarterlyReportID}")]
        [HttpGet]
        public IList<Staffing> Get(string quarterlyReportID)
        {
            var result = this.staffService.Get(quarterlyReportID);
            return result;
        }

        /// <summary>
        /// Validate Staffing records for given quarterly report ID.
        /// </summary>
        /// <param name="quarterlyReportID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Owner, Contributor")]
        [Route("validate/{quarterlyReportID}")]
        [HttpGet]
        public IList<ValidationResult> Validate(string quarterlyReportID)
        {
            var result = this.staffService.Validate(quarterlyReportID);
            return result;
        }
    }
}
