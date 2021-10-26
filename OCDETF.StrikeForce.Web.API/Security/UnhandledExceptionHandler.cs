using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Security
{
    public class UnhandledExceptionHandler
    {
        private readonly ILogger logger;
        private readonly RequestDelegate next;

        public UnhandledExceptionHandler(ILogger<UnhandledExceptionHandler> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                File.WriteAllText("unhandled.txt", exception.StackTrace, Encoding.UTF8);
                logger.LogError(exception,
                    $"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");
            }
        }
    }
}
