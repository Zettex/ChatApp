using ChatApp.Models;
using System.Collections.Generic;

namespace ChatApp.Repositories
{
    public interface IChatRepository
    {
        string Login(string username, string password);
        User GetUser(int id, string username = null);
        string GetUserRole(int id, string username = null);
        IList<Room> GetRooms();
        IList<User> GetUsers();
        Room GetRoom(int id);
        void DeleteRoom(Room room);
        IList<Message> GetMessagesFromRoomId(int id);
        void AddUser(User user);
        void DeleteUser(User user);
        Message GetMessage(int id);
        int AddMessage(Message msg);
        void DeleteMessage(int id);
        int AddRoom(Room room);
        void EditUser(User user);
        void EditRoom(Room room);
    }
}
