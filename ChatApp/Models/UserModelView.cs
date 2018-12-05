using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class UserModelView
    {
        public UserModelView(User user, User currentUser)
        {
            User = user;
            CurrentUser = currentUser;
        }

        public User User
        { get; set; }

        public User CurrentUser
        { get; set; }
    }
}