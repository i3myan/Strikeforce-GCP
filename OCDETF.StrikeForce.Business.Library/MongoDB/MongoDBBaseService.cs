using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.MongoDB
{
    public class MongoDBBaseService
    {
        protected IMongoClient MongoClient { get; set; }
        protected IMongoDatabase MongoDatabase { get; set; }

        protected ILogger Logger { get; set; }

        protected AppConfiguration AppConfig { get; set; }

        public MongoDBBaseService() { }

        public MongoDBBaseService(IMongoClient mongClient, ILogger logger, AppConfiguration appConfig)
        {
            this.MongoClient = mongClient;
            this.Logger = logger;
            this.AppConfig = appConfig;
        }
    }
}
