using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library
{
    public interface IUserService
    {
        public User Add(User newUser, string userID);
        public IList<User> GetAllUsers();
        public User Update(User newUser, string userID);
        public User Get(string userName);
        public User Login(string userName, string passWord);
    }
}
