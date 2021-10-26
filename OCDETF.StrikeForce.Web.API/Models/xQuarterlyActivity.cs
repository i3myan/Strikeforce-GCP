using AutoMapper.Configuration.Annotations;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Models
{
    public class xQuarterlyActivity
    {
        public string ID { get; set; }
        public string StrikeForceID { get; set; }
        public string ActivityName { get; set; }
        public string FirstQuarter { get; set; }
        public string SecondQuarter { get; set; }
        public string ThirdQuarter { get; set; }
        public string FourthQuarter { get; set; }
        public string Total { get; set; }
        public string Category { get; set; }

        public string CreatedUserID { get; set; }
        public string UpdatedUserID { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        [Ignore]
        public int Order { get; set; }
        [Ignore]
        public bool Delete { get; set; }
        
    }
}
