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
    [Route("api/recent")]
    [ApiController]
    public class RecentDevelopmentsController : ControllerBase
    {
        private IRecentDevelopmentService RecentDevelopmentService { get; set; }
        
        public RecentDevelopmentsController(IRecentDevelopmentService recentDevelopmentService)
        {
            this.RecentDevelopmentService = recentDevelopmentService;                        
        }

        [Authorize(Roles = "Administrator, Owner")]
        [Route("create-new")]
        [HttpPost]
        public IList<RecentDevelopments> Add(RecentDevelopments recentItem)
        {
            var result = this.RecentDevelopmentService.Add(recentItem, HttpContext.User.Identity.Name);

            return result;
        }

        [Authorize(Roles = "Administrator, Owner")]
        [Route("update-case")]
        [HttpPost]
        public IList<RecentDevelopments> Update(RecentDevelopments recentItem)
        {
            var result = this.RecentDevelopmentService.Update(recentItem, HttpContext.User.Identity.Name);

            return result;
        }

        [Authorize(Roles = "Administrator, Owner")]
        [Route("delete-item")]
        [HttpPost]
        public IList<RecentDevelopments> Delete(RecentDevelopments recentItem)
        {
            var result = this.RecentDevelopmentService.Delete(recentItem, HttpContext.User.Identity.Name);
            return result;
        }


        [Authorize(Roles = "Administrator, Owner")]
        [Route("get-all/{quarterlyReportID}")]
        [HttpGet]
        public IList<RecentDevelopments> Get(string quarterlyReportID)
        {
            var result = this.RecentDevelopmentService.Get(quarterlyReportID);

            return result;
        }
    }
}
