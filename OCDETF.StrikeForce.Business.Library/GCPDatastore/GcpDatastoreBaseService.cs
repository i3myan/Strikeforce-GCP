using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Business.Library.Models;
using Google.Api.Gax;
using Google.Cloud.Datastore.V1;


namespace OCDETF.StrikeForce.Business.Library
{
    public class GcpDatastoreBaseService
    {
        protected ILogger Logger { get; set; }
        protected AppConfiguration AppConfig { get; set; }
        protected DatastoreDb datastoreDb { get; set; }

        public GcpDatastoreBaseService() { }

        public GcpDatastoreBaseService(DatastoreDb db, ILogger logger)
        {
            this.datastoreDb = db;
            this.Logger = logger;
        }

        public GcpDatastoreBaseService(DatastoreDb db, AppConfiguration appConfig, ILogger logger)
        {
            this.datastoreDb = db;
            this.AppConfig = appConfig;
            this.Logger = logger;
        }
    }
}
