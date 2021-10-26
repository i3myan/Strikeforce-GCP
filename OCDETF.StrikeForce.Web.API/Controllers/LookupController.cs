using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCDETF.StrikeForce.Business.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Business.Library.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("api/lookup")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private ILookupService LookupService { get; set; }
        public LookupController(ILookupService lookupService) {
            this.LookupService = lookupService;
        }

        [Authorize]
        [HttpPost]
        [Route("add-strike-force")]
        public StrikeForceNames AddStrikeForce(StrikeForceNames force)
        {
            return force;
        }

        [Authorize]
        [HttpGet]
        [Route("all-forces")]
        public IList<StrikeForceNames> GetAllStrikeForces()
        {
            return this.LookupService.GetForceLocations();
        }
    }
}
