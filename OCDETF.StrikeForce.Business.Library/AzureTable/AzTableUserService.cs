using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library.AzureTable
{
    public class AzTableUserService : AzTableBaseService, IUserService
    {        
        public AzTableUserService() { }

        public AzTableUserService(TableServiceClient serviceClient, ILogger logger):base(serviceClient, logger)
        {            
            this.AzureTableService.CreateTableIfNotExists(StrikeForceTables.Users.ToString());
            TableClient = this.AzureTableService.GetTableClient(StrikeForceTables.Users.ToString());
        }

        public User Add(User newUser, string userID)
        {

            this.AppLogger.LogInformation("Adding User:", newUser, userID);
            var result = Get(newUser.Email.Trim());
            if (result ==null)
            {
                newUser.DateCreated = DateTime.Now;
                newUser.CreatedUserID = userID;
                newUser.ID = Guid.NewGuid().ToString();
                newUser.IsActive = true;
                newUser.RowKey = newUser.ID;
                newUser.Email = newUser.Email.Trim();
                newUser.PartitionKey = newUser.StrikeForceID;
                newUser.LastLoginDate = DateTime.Now;

                TableClient.AddEntity<User>(newUser);
            }
            

            return newUser;
        }

        /// <summary>
        /// Returns a user details if found, else returns null
        /// </summary>
        /// <param name="userName">Username is email</param>
        /// <returns>User object</returns>
        public User Get(string userName)
        {
            this.AppLogger.LogInformation("Checking If User Exists: " + userName);
            User authUser = null;
            TableClient tableClient = AzureTableService.GetTableClient("Users");


            Pageable<User> queryResultsFilter = tableClient.Query<User>(filter: $"Email eq '{userName}' ");
            foreach (User qEntity in queryResultsFilter)
            {
                authUser = tableClient.GetEntity<User>(qEntity.PartitionKey, qEntity.RowKey);
                break;
            }


            return authUser;
        }

        public IList<User> GetAllUsers()
        {
            this.AppLogger.LogInformation("Get All Users");
            var result = new LookupService(AzureTableService).GetAllForces();
            IList<User> allUsers = new List<User>();
            Pageable<User> queryResultsFilter = TableClient.Query<User>();
            foreach (User qEntity in queryResultsFilter)
            {
                var temp = result.Where(sel => sel.ID == qEntity.StrikeForceID).FirstOrDefault();
                if (temp != null)
                    qEntity.StrikeForceID = temp.Name;
                allUsers.Add(qEntity);
            }
            return allUsers;
        }

        public User Login(string userName, string passWord)
        {
            this.AppLogger.LogInformation(string.Format("Login Attempt {0}", userName));
            User authUser = null;
            TableClient tableClient = AzureTableService.GetTableClient("Users");


            Pageable<User> queryResultsFilter = tableClient.Query<User>(filter: $"Email eq '{userName}' and Password eq '{passWord}'");
            foreach (User qEntity in queryResultsFilter)
            {
                authUser = tableClient.GetEntity<User>(qEntity.PartitionKey, qEntity.RowKey);
                break;
            }


            return authUser;
        }

        public User Update(User newUser, string userID)
        {
            this.AppLogger.LogInformation(string.Format("Updating User {0}", newUser.Email), newUser);
            newUser.UpdatedUserID = userID;
            newUser.DateUpdated = DateTime.Now;
            TableClient.UpsertEntity<User>(newUser);

            return newUser;
        }
    }
}
