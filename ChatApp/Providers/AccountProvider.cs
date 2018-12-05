using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ChatApp.Context;
using ChatApp.Models;
using ChatApp.Repositories;

namespace ChatApp.Providers
{
    public class AccountProvider : IAccountProvider
    {
        /// <summary>
        /// Return True if the user is already logged-in.
        /// </summary>
        public bool IsLoggedIn => HttpContext.Current.User.Identity.IsAuthenticated;

        public string GetUserRole(IChatRepository chatRepository, int id, string username = null) => 
            chatRepository.GetUserRole(id, username);

        /// <summary>
        /// Authenticate an user and set cookie if user is valid.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(IChatRepository chatRepository, string username, string password)
        {
            string result = chatRepository.Login(username, password); // TODO: User Membership APIs

            if (result == null)
                return false;

            FormsAuthentication.SetAuthCookie(username, false);
            return true;
        }

        public User GetUser(IChatRepository chatRepository)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                return chatRepository.GetUser(0, HttpContext.Current.User.Identity.Name);
           
            return null;
        }

        /// <summary>
        /// Logout the user.
        /// </summary>
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}