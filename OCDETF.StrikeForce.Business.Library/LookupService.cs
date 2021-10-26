using Azure;
using Azure.Data.Tables;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Business.Library.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library
{
    public class LookupService
    {
        
        private TableServiceClient AzureTableService { get; set; }
        private TableClient TableClient { get; set; }
        
        public LookupService() { }
        public LookupService(TableServiceClient serviceClient)
        {
            this.AzureTableService = serviceClient;
            TableClient = this.AzureTableService.GetTableClient(StrikeForceTables.StrikeForceLocations.ToString());
        }

        public StrikeForceNames Add(StrikeForceNames forceName, string userID)
        {
            forceName.DateCreated = DateTime.Now;
            forceName.CreatedUserID = userID;
            forceName.ID = Guid.NewGuid().ToString();
            forceName.IsActive = true;

            forceName.RowKey = forceName.ID;
            forceName.PartitionKey = forceName.Name;


            TableClient.AddEntity<StrikeForceNames>(forceName);
            return forceName;
        }

        public IList<StrikeForceNames> GetAllForces()
        {
            IList<StrikeForceNames> allUsers = new List<StrikeForceNames>();
            Pageable<StrikeForceNames> queryResultsFilter = TableClient.Query<StrikeForceNames>();
            foreach (StrikeForceNames qEntity in queryResultsFilter)
            {
                allUsers.Add(qEntity);
            }
            return allUsers;
        }
    }
}
