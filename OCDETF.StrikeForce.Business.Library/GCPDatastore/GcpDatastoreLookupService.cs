
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Business.Library.Models.Lookups;
using Google.Api.Gax;
using Google.Cloud.Datastore.V1;
using MongoDB.Driver;

namespace OCDETF.StrikeForce.Business.Library
{
    public class GcpDatastoreLookupService : GcpDatastoreBaseService, ILookupService
    {
     
        private IList<StrikeForceNames> StrikeForceLocations { get; set; }

        public GcpDatastoreLookupService() { }

        public GcpDatastoreLookupService(DatastoreDb db, ILogger logger, AppConfiguration appConfig) : base(db, appConfig, logger)
        {
            this.datastoreDb = db;
        }
        public IList<StrikeForceNames> GetForceLocations()
        {
            Query query = new Query("StrikeForceLocations")
            {
                Order = { { "Name", PropertyOrder.Types.Direction.Ascending } }
            };

            List<StrikeForceNames> sfNames = this.datastoreDb.RunQuery(query).Entities.Select(e => e.ToStrikeForceNames()).ToList();
            return sfNames;

        }


        public StrikeForceNames AddForce(StrikeForceNames forceLocation, string userID)
        {
            forceLocation.UpdatedUserID = userID;
            forceLocation.CreatedUserID = userID;
            forceLocation.DateCreated = DateTime.UtcNow;
            forceLocation.DateUpdated = DateTime.UtcNow;
            forceLocation.IsActive = true;
        
            this.datastoreDb.Insert(forceLocation.ToStrikeForceNamesEntity(this.datastoreDb));    
            return forceLocation;
        }
    }
}
