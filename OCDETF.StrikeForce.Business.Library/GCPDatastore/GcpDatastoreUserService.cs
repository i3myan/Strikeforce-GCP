using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using OCDETF.StrikeForce.Business.Library.Models;
using Google.Api.Gax;
using Google.Cloud.Datastore.V1;

namespace OCDETF.StrikeForce.Business.Library
{
    public class GcpDatastoreUserService : GcpDatastoreBaseService, IUserService
    {
        protected KeyFactory keyFactory { get; set; }
        protected IList<User> UserList { get; set; }
        public GcpDatastoreUserService() { }

        public GcpDatastoreUserService(DatastoreDb db, ILogger logger) : base(db, logger)
        {
            this.datastoreDb = db;
            this.keyFactory = this.datastoreDb.CreateKeyFactory(StrikeForceTables.Users.ToString());
            this.UserList = this.GetAllUsers();
        }

        public User Add(User newUser, string userID)
        {
            this.Logger.LogInformation("Adding User:", newUser, userID);
            var result = Get(newUser.Email.Trim());
            if (result == null)
            {
                this.datastoreDb.Insert(newUser.ToUserEntity(this.datastoreDb));
            }
            return newUser;
        }

        public User Get(string userName)
        {
            this.Logger.LogInformation("Checking If User Exists: " + userName);
            User authUser = this.UserList.FirstOrDefault(u => u.Email.ToLower() == userName.ToLower());
            return authUser;
        }

        public IList<User> GetAllUsers()
        {
            Query query = new Query(StrikeForceTables.Users.ToString())
            {
                //Order = { { "LastName", PropertyOrder.Types.Direction.Ascending } }
            };

            List<User> users = this.datastoreDb.RunQuery(query).Entities.Select(e => e.ToUser()).OrderBy(e => e.LastName).ToList(); 
            return users;
        }

        public User Login(string userName, string passWord)
        {
            this.Logger.LogInformation(string.Format("Login Attempt {0}", userName));
            return this.UserList.FirstOrDefault(u => u.Email.ToLower() == userName.ToLower() && u.Password == passWord);
        }

        public User Update(User newUser, string userID)
        {          
            this.datastoreDb.Update(newUser.ToUserEntity(this.datastoreDb));
            return newUser;
        }

    }
}
