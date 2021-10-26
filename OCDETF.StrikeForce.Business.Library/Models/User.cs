using Azure;
using Azure.Data.Tables;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library.Models
{
    public class User : ITableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
        
        public string CreatedUserID { get; set; }
        public string UpdatedUserID { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }


        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        [BsonIgnore]
        public ETag ETag { get; set; }        

    }
}
