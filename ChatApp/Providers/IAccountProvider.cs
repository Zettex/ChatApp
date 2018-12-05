using ChatApp.Models;
using ChatApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Providers
{
    public interface IAccountProvider
    {
        /// <summary>
        /// Return True if the user is already logged-in.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Authenticate an user and set cookie if user is valid.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Login(IChatRepository chatRepository, string username, string password);

        /// <summary>
        /// Logout the user.
        /// </summary>
        void Logout();

        string GetUserRole(IChatRepository chatRepository, int id, string username = null);

        User GetUser(IChatRepository chatRepository);
    }
}
