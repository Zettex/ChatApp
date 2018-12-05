using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class MessageTypingModelView
    {
        public MessageTypingModelView(User user, string userId)
        {
            User = user;
            UserId = userId;
        }

        public User User
        { get; private set; }

        public string UserId
        { get; private set; }
    }
}