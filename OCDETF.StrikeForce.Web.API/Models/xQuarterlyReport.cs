using AutoMapper.Configuration.Annotations;
using Azure;
using Azure.Data.Tables;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Web.API.Models
{
    public class xQuarterlyReport 
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string StrikeForceName { get; set; }
        public string StrikeForceID { get; set; }
        public string FiscalYear { get; set; }
        public string Quarter { get; set; }     
        public string OperationsBegin { get; set; }
        public string MOUDate { get; set; }
        public string HistoryNotes { get; set; }
        public string Mission { get; set; }
        public string Structure { get; set; }
        public string OfficeLocations { get; set; }

        public string Challenges { get; set; }
        public string HeadsUp { get; set; }
        public string SeizureNotes { get; set; }
        public string SpecificNotes { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }

        [Ignore]
        public IList<Staffing> StaffingData { get; set; }
        [Ignore]
        public IList<QuarterlyActivity> RequiredData { get; set; }
        [Ignore]
        public IList<QuarterlyActivity> OCDETFCases { get; set; }
        [Ignore]
        public IList<QuarterlyActivity> SeizuresData { get; set; }
        [Ignore]
        public IList<QuarterlyActivity> SpecificData { get; set; }        
        [Ignore]
        public IList<RecentDevelopments> RecentDevelopments { get; set; }

        public string CreatedUserID { get; set; }
        public string UpdatedUserID { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }        
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
