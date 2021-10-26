using AutoMapper;
using OCDETF.StrikeForce.Business.Library;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Web.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.AutoMapper
{
    public class StrikeForceMapperConfig : Profile
    {
        public StrikeForceMapperConfig()
        {
            CreateMap<User, xUser>();
            CreateMap<xQuarterlyReport, QuarterlyReport>();
            CreateMap<QuarterlyReport, xQuarterlyReport>();
            
        }
    }
}
