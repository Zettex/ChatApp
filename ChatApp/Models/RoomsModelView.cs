using ChatApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class RoomsModelView
    {
        public RoomsModelView(IChatRepository chatRepository, User user)
        {
            Rooms = chatRepository.GetRooms();
            User = user;
        }

        public IList<Room> Rooms
        { get; private set; }

        public User User
        { get; private set; }
    }
}