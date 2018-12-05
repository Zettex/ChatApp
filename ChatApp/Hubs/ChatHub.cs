using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ChatApp.Controllers;
using ChatApp.Models;
using Microsoft.AspNet.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public static void AddMessage(string roomId, Message msg)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            context.Clients.Group(roomId).addMessage(msg);
        }

        //public static void Typing(string roomId, string id, string result, bool stop)
        //{
        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

        //    context.Clients.Group(roomId, id).typing(result, id, stop);
        //}

        public static void DeleteMessage(string messageId, string roomId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            context.Clients.Group(roomId).deleteMessage(messageId);
        }

        public static void AddRoom(string result)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            context.Clients.All.addRoom(result);
        }

        public Task JoinRoom(string roomId)
        {
            return Groups.Add(Context.ConnectionId, roomId);
        }

        public Task LeaveRoom(string roomId)
        {
            return Groups.Remove(Context.ConnectionId, roomId);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}