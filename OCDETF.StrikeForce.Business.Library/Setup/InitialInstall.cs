using Azure;
using Azure.Data.Tables;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library.Setup
{
    public class InitialInstall
    {
        private TableServiceClient azureTableService { get; set; }

        public InitialInstall(TableServiceClient serviceClient)
        {
            this.azureTableService = serviceClient;
        }

        public InitialInstall() { }

        public void Setup()
        {
            try
            {
                // Creates a table.
                azureTableService.CreateTableIfNotExists(StrikeForceTables.Users.ToString());
                azureTableService.CreateTableIfNotExists(StrikeForceTables.QuarterlyReports.ToString());
                azureTableService.CreateTableIfNotExists(StrikeForceTables.OCDETF.ToString());
                azureTableService.CreateTableIfNotExists(StrikeForceTables.Staffing.ToString());
                azureTableService.CreateTableIfNotExists(StrikeForceTables.Specific.ToString());
            
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine("Create existing table throws the following exception:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
