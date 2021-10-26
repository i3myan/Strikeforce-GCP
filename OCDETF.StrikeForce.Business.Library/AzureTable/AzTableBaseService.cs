using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.AzureTable
{
    public class AzTableBaseService
    {
        protected ILogger AppLogger { get; set; }
        protected AppConfiguration AppConfig { get; set; }
        protected TableServiceClient AzureTableService { get; set; }
        protected TableClient TableClient { get; set; }

        public AzTableBaseService() { }

        public AzTableBaseService(TableServiceClient azureTableService, ILogger logger)
        {
            this.AzureTableService = azureTableService;
            this.AppLogger = logger;
        }

        
        public AzTableBaseService(TableServiceClient azureTableService, AppConfiguration appConfig, ILogger logger)
        {
            this.AzureTableService = azureTableService;
            this.AppConfig = appConfig;
            this.AppLogger = logger;
        }
    }
}
