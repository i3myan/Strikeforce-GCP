using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Business.Library.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.AzureTable
{
    public class AzTableLookupService : AzTableBaseService, ILookupService
    {
        public AzTableLookupService(TableServiceClient serviceClient, ILogger logger):base(serviceClient, logger)
        {            
            this.TableClient = this.AzureTableService.GetTableClient(StrikeForceTables.StrikeForceLocations.ToString());
        }

        public StrikeForceNames AddForce(StrikeForceNames forceLocation, string userID)
        {
            throw new NotImplementedException();
        }

        public IList<StrikeForceNames> GetForceLocations()
        {            
            Pageable<StrikeForceNames> queryResultsFilter = this.TableClient.Query<StrikeForceNames>();
            
            return queryResultsFilter.ToList();
        }
    }
}
