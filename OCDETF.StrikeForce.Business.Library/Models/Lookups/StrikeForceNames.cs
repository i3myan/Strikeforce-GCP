using Azure;
using Azure.Data.Tables;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OCDETF.StrikeForce.Business.Library.Models.Lookups
{
    public class StrikeForceNames : ITableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public string Name { get; set; }
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
