using ChatApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class ChatModelView
    {
        public ChatModelView(IChatRepository chatRepository, int roomId, User user)
        {
            Messages = chatRepository.GetMessagesFromRoomId(roomId);
            Room = chatRepository.GetRoom(roomId);
            User = user;
        }

        public IList<Message> Messages
        { get; private set; }

        public User User
        { get; private set; }

        public Room Room
        { get; private set; }
        
    }
}