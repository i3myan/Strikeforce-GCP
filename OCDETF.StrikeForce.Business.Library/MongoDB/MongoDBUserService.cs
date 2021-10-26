using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.MongoDB
{
    public class MongoDBUserService: MongoDBBaseService, IUserService
    {
        private IMongoCollection<User> UserList { get; set; }
        private ILookupService LookupService { get; set; }

        public MongoDBUserService() { }

        public MongoDBUserService(IMongoClient mongoClient, ILookupService lookupService, AppConfiguration myConfig, ILogger logger) : base(mongoClient, logger, myConfig)
        {
            this.LookupService = lookupService;

            MongoDatabase = mongoClient.GetDatabase(AppConfig.MongoDBName);
            UserList = MongoDatabase.GetCollection<User>(StrikeForceTables.Users.ToString());
        }

        public User Add(User newUser, string userID)
        {
            this.Logger.LogInformation("Adding User:", newUser, userID);
            var result = Get(newUser.Email.Trim());
            if (result == null)
            {
                newUser.DateCreated = DateTime.Now;
                newUser.CreatedUserID = userID;
                newUser.IsActive = true;

                newUser.Email = newUser.Email.Trim();
                newUser.LastLoginDate = DateTime.Now;

                UserList.InsertOne(newUser);
            }


            return newUser;
        }

        public User Get(string userName)
        {
            this.Logger.LogInformation("Checking If User Exists: " + userName);
            User authUser = UserList.Find(sel => sel.Email == userName).FirstOrDefault();

            return authUser;
        }

        public IList<User> GetAllUsers()
        {
            this.Logger.LogInformation("Get All Users");
            var result = this.LookupService.GetForceLocations();
            IList<User> allUsers = UserList.Find(sel => true).ToList();
            foreach (User qEntity in allUsers)
            {
                var temp = result.Where(sel => sel.ID == qEntity.StrikeForceID).FirstOrDefault();
                if (temp != null)
                    qEntity.StrikeForceID = temp.Name;
            }
            return allUsers;
        }

        public User Login(string userName, string passWord)
        {
            this.Logger.LogInformation(string.Format("Login Attempt {0}", userName));

            return UserList.Find(sel => sel.Email == userName && sel.Password == passWord).FirstOrDefault();            
        }

        public User Update(User newUser, string userID)
        {
            this.Logger.LogInformation(string.Format("Updating User {0}", newUser.Email), newUser);
            newUser.UpdatedUserID = userID;
            newUser.DateUpdated = DateTime.Now;
            UserList.ReplaceOne(sel => sel.ID == newUser.ID, newUser);

            return newUser;
        }
    }
}
