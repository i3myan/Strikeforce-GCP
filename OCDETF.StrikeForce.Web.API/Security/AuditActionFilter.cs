using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Security
{
    public class AuditActionFilter : Attribute, IActionFilter
    {
        private readonly ILogger AppLogger;



        public AuditActionFilter(ILogger ilogger)
        {
            this.AppLogger = ilogger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
            this.AppLogger.LogInformation("Action Executing!");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            this.AppLogger.LogInformation("Action Executing!");
        }
    }
}
