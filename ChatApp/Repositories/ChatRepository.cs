using ChatApp.Context;
using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ChatApp.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private ChatContext _context = new ChatContext();

        public int AddMessage(Message msg)
        {
            _context.Messages.Add(msg);
            _context.SaveChanges();
            return msg.Id;
        }
        
        public int AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return room.Id;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void EditUser(User user)
        {
            var result = GetUser(user.Id);
            if (result != null)
            {
                result.Name = user.Name;
                result.ImageUrl = user.ImageUrl;
                result.Password = user.Password;
                if (user.Role != null)
                {
                    result.Role = user.Role;
                    result.CanCreateRoom = user.CanCreateRoom;
                    result.CanWriteMessages = user.CanWriteMessages;
                }
            }

            _context.SaveChanges();
        }

        public IList<Message> GetMessagesFromRoomId(int id)
        {
            return _context.Messages.Where(m => m.Room.Id == id).ToList();
        }

        public Room GetRoom(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.Id == id);
        }

        public IList<Room> GetRooms()
        {
            return _context.Rooms.ToList();
        }

        public User GetUser(int id, string username = null)
        {
            if (username != null)
            {
                return _context.Users.FirstOrDefault(u => u.Name == username);
            }

            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public string GetUserRole(int id, string username = null)
        {
            if (username != null)
            {
                return _context.Users.FirstOrDefault(u => u.Name == username).Role;
            }

            return _context.Users.FirstOrDefault(u => u.Id == id).Role;
        }

        public string Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username && u.Password == password);

            return user != null ? user.Name : null;
        }

        public IList<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void DeleteRoom(Room room) 
        {
            _context.Entry(room).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Entry(user).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void EditRoom(Room room)
        {
            var result = GetRoom(room.Id);
            if (result != null)
            {
                result.Name = room.Name;
            }

            _context.SaveChanges();
        }

        public void DeleteMessage(int id)
        {
            Message msg = GetMessage(id);
            _context.Entry(msg).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Message GetMessage(int id)
        {
            return _context.Messages.FirstOrDefault(m => m.Id == id);
        }
    }
}