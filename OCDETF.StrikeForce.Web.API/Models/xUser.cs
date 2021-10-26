using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Models
{
    public class xUser
    {
        public string ID { get; set; }
        public string StrikeForceID { get; set; }
        public string Email { get; set; }        
        public string Password { get; set; }
        public bool Contributor { get; set; }
        public bool Administrator { get; set; }
        public bool Owner { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIPAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreatedUserID { get; set; }
        public string UpdatedUserID { get; set; }

        [Ignore]
        public string StrikeForceName { get; set; }

        [Ignore]
        public string Session { get; set; }

        [Ignore]
        public bool IsAuthenticated { get; set; }
    }
}
