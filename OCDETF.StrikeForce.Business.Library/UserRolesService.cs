using Azure.Data.Tables;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library
{
    public class UserRolesService
    {
        private TableServiceClient AzureTableService { get; set; }
        private TableClient TableClient { get; set; }
        public UserRolesService() { }
        public UserRolesService(TableServiceClient serviceClient)
        {
            this.AzureTableService = serviceClient;
            TableClient = this.AzureTableService.GetTableClient(StrikeForceTables.UserRoles.ToString());
        }

        public bool Add(UserRoles userRole, string requestUserID)
        {
            userRole.CreatedUserID = requestUserID;
            userRole.DateCreated = DateTime.Now;
            
            var result = TableClient.AddEntity<UserRoles>(userRole);
            if (result.Status == 200)
                return true;
            else
                return false;
        }

        public bool Update(UserRoles userRole, string requestUserID)
        {
            userRole.CreatedUserID = requestUserID;
            userRole.DateCreated = DateTime.Now;

            var result = TableClient.UpdateEntity<UserRoles>(userRole, Azure.ETag.All, TableUpdateMode.Merge);
            if (result.Status == 200)
                return true;
            else
                return false;
        }
    }
}
