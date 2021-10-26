using Azure;
using Azure.Data.Tables;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OCDETF.StrikeForce.Business.Library.Models
{
    public class RecentDevelopments :ITableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public string StrikeForceID { get; set; }
        public string QuarterlyReportID { get; set; }
        public string CaseType { get; set; }
        public string AgencyName { get; set; }
        public string SponsorAgency { get; set; }
        public string Summary { get; set; }
        
        public int  Order { get; set; }
        public bool IsActive { get; set; }

        public string CreatedUserID { get; set; }
        public string UpdatedUserID { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public ETag ETag { get; set; }
    }
}
