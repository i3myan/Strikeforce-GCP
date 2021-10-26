using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library.Models
{
    public class Roles : ITableEntity
    {
        public string ID { get; set; }
        public string RoleName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreatedUserID { get; set; }
        public string UpdatedUserID { get; set; }
        public bool Active { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
