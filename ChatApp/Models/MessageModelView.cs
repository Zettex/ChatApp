using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class MessageModelView
    {
        public MessageModelView(Message message, User user)
        {
            Message = message;
            User = user;
        }

        public Message Message
        { get; private set; }

        public User User
        { get; private set; }
    }
}