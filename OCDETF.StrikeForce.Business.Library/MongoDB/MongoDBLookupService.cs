using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Business.Library.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.MongoDB
{
    public class MongoDBLookupService : MongoDBBaseService, ILookupService
    {
        private IMongoCollection<StrikeForceNames> StrikeForceLocations { get; set; }

        public MongoDBLookupService() { }

        public MongoDBLookupService(IMongoClient mongoClient, ILogger logger, AppConfiguration appConfig) : base(mongoClient, logger, appConfig)
        {
            MongoDatabase = mongoClient.GetDatabase(appConfig.MongoDBName);
            StrikeForceLocations = MongoDatabase.GetCollection<StrikeForceNames>(StrikeForceTables.StrikeForceLocations.ToString());
        }
        public IList<StrikeForceNames> GetForceLocations()
        {
            return StrikeForceLocations.Find<StrikeForceNames>(sel => true).ToList();
        }

        public StrikeForceNames AddForce(StrikeForceNames forceLocation, string userID)
        {
            forceLocation.UpdatedUserID = userID;
            forceLocation.CreatedUserID = userID;
            forceLocation.DateCreated = DateTime.Now;
            forceLocation.DateUpdated = DateTime.Now;
            forceLocation.IsActive = true;

            StrikeForceLocations.InsertOne(forceLocation);
            return forceLocation;
        }
    }
}
