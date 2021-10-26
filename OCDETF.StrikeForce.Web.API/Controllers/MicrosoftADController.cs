using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    [ApiController]
    public class MicrosoftADController : ControllerBase
    {
        private IUserService UserService { get; set; }
        private LookupService LookupService { get; set; }
        private AppConfiguration AppConfig { get; set; }
        private ILogger Logger { get; set; }

        public IMapper MyAutoMapper { get; set; }

        public MicrosoftADController(IUserService userService,AppConfiguration appConfig, LookupService lookupService, IMapper mapper, ILogger logger)
        {
            this.Logger = logger;
            this.AppConfig = appConfig;
            this.UserService = userService;
            this.LookupService = lookupService;
            this.MyAutoMapper = mapper;
        }

        //[HttpGet]        
        //[Route("")]
        //public IActionResult Index()
        //{
        //    return Redirect("https://localhost:5001/index.html");
        //}

        [HttpGet]
        [Authorize]
        [Route("api/AD/User")]
        public xUser GetUser()
        {
            xUser aUser = new xUser();
            string userName = string.Empty;
            if (this.AppConfig.AuthenticationMethod == "Okta")
            {
                foreach (var aClaim in HttpContext.User.Claims)
                {
                    if (aClaim.Type == "preferred_username")
                        userName = aClaim.Value;
                }
            }
            
            
            this.Logger.LogInformation("User Name" + userName);
            var result = UserService.Get(userName);
            MyAutoMapper.Map(result, aUser);
            aUser.IsAuthenticated = true;

            var strikeForces = LookupService.GetAllForces();
            var matchStrikeForce = strikeForces.Where(sel => sel.ID == result.StrikeForceID).FirstOrDefault();
            if (matchStrikeForce != null)
                aUser.StrikeForceName = matchStrikeForce.Name;

            
            return aUser;            
        }

        [HttpGet]
        [Authorize]
        [Route("api/AD/logout")]
        public bool Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return true;
        }
    }
}
