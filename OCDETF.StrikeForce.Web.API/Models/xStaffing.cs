using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Models
{
    public class xStaffing
    {
        public string ID { get; set; }
        public string StrikeForceID { get; set; }
        public string AgencyName { get; set; }
        public int NumberOfAgents { get; set; }
        public int NumberOfFederalTFOs { get; set; }
        public int NumberOfAnalysts { get; set; }
        public int OtherNumbers { get; set; }

        public int Total { get; set; }
        public bool Delete { get; set; }
        public int Order { get; set; }
    }
}
