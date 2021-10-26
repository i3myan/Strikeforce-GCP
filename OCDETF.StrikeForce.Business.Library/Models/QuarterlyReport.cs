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
    public class QuarterlyReport :ITableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string StrikeForceID { get; set; }
        public string StrikeForceName { get; set; }        
        public int Quarter { get; set; }
        public int FiscalYear { get; set; }
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
