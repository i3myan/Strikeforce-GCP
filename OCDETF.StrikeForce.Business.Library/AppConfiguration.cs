using AutoMapper;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library
{
    public class AppConfiguration
    {
        public string AuthenticationMethod { get; set; }
        public string AzureStorageAccountName { get; set; }
        public string AzureStorageAccountURL { get; set; }
        public string AzureStorageAccountKey { get; set; }
        public string MongoDBName { get; set; }
        public string MongoDBConnection { get; set; }
        public string GCProjectId { get; set; }
        public string GCProjectNumber { get; set; }
        public string JwtSecret { get; set; }
        public string CORS { get; set; }
        public string InstrumentationKey { get; set; }
        public IList<Staffing> Staffing { get; set; }
        public IList<QuarterlyActivity> Required { get; set; }
        public IList<QuarterlyActivity> Specific { get; set; }
        public IList<QuarterlyActivity> Seizures { get; set; }
        public IList<QuarterlyActivity> OCDETF { get; set; }
    }
}
