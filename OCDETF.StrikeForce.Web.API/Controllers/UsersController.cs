using AutoMapper;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCDETF.StrikeForce.Business.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Web.API.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OCDETF.StrikeForce.Web.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILookupService lookupService;
        private readonly AppConfiguration AppConfig;
        private readonly IMapper myAutoMapper;
        private readonly ILogger logger;

        public UsersController(IUserService userAccountsService, ILookupService lookupService, AppConfiguration appConfig, Serilog.ILogger applogger, IMapper mapper)
        {
            this.userService = userAccountsService;
            this.AppConfig = appConfig;
            this.myAutoMapper = mapper;
            this.logger = applogger;
            this.lookupService = lookupService;
        }


        /// <summary>
        /// Login to authenticate.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public xUser Login(User newUser)
        {
            //this.logger.LogInformation("Login :", newUser);
            //Log.Logger = new LoggerConfiguration().CreateLogger();
            this.logger.Information("No one listens to me! {@newUser}", newUser);

            xUser aUser = new xUser();
            var result2 = lookupService.GetForceLocations();

            var result = userService.Login(newUser.Email, newUser.Password);
            if (result != null)
            {
                //this.logger.LogDebug("Login User:", aUser.Email);
                myAutoMapper.Map(result, aUser);

                //generate token
                aUser.Session = new JwtTokenProvider(this.AppConfig).generateJwtToken(result);
                aUser.IsAuthenticated = true;

                var temp = result2.Where(sel => sel.ID == aUser.StrikeForceID).FirstOrDefault();
                if (temp != null)
                    aUser.StrikeForceName = temp.Name;

                //update login date and ipaddress
                result.LastLoginIPAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                result.LastLoginDate = DateTime.Now;
                this.userService.Update(result, newUser.Email);
            }
            return aUser;
        }


        /// <summary>
        /// retrieve a user based on username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        ///  [Authorize(Roles = "Administrator, Owner, Contributor")]
        [HttpGet]
        [Route("get")]
        public User GetUser(string userName)
        {
            return this.userService.Get(userName);
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Administrator")]
        [HttpPost("add-user")]
        public User Add(User newUser)
        {
            newUser.Email = newUser.Email.Trim();
            newUser.Password = newUser.Password.Trim();
            var result = this.userService.Add(newUser, HttpContext.User.Identity.Name);
            return result;
        }

        /// <summary>
        /// Update user information.
        /// </summary>
        /// <param name="newUser"></param>
        [Authorize(Roles = "Administrator")]
        [HttpPut("update-user")]
        public void Update(User newUser)
        {
            this.userService.Update(newUser, HttpContext.User.Identity.Name);
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("all")]
        public IList<User> GetAllUsers()
        {
            return this.userService.GetAllUsers();
        }

        //[Authorize(Roles = "Administrator, Owner, Contributor")]
        [HttpGet]
        [Route("logout")]
        public void Logout(xUser newUser)
        {
           // this.logger.LogDebug("Logout User:", newUser.Email);
        }
    }
}
