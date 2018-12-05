using ChatApp.Hubs;
using ChatApp.Models;
using ChatApp.Providers;
using ChatApp.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IChatRepository _chatRepository;

        public ChatController(IAccountProvider authProvider, IChatRepository chatRepository)
        {
            _accountProvider = authProvider;
            _chatRepository = chatRepository;
        }
        
        public ActionResult Room(int id = 0)
        {
            if (id != 0)
            {
                if (_chatRepository.GetRoom(id) == null)
                    throw new HttpException(404, "HTTP/1.1 404 Not Found");

                var user = _accountProvider.GetUser(_chatRepository);
                // Warning: if SuperAdmin have CanWriteMessages/CanCreateRoom = false, then will throw Error
                ChatModelView model = new ChatModelView(_chatRepository, id, user);

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public void AddMessage(Message model)
        {
            // The user that sent the message
            model.User = _chatRepository.GetUser(model.UserId);
            model.Date = DateTime.UtcNow;

            var id = _chatRepository.AddMessage(model);
            model.Id = id;

            ChatHub.AddMessage(model.RoomId.ToString(), model);
        }

        [HttpPost]
        public PartialViewResult GetMessageView(Message message)
        {
            var currentUser = _accountProvider.GetUser(_chatRepository);

            MessageModelView MMV = new MessageModelView(message, currentUser);
            return PartialView("_Message", MMV);
        }
        
        //[HttpPost]
        //public void Typing(string roomId, string id, bool stopTyping)
        //{
        //    var user = _accountProvider.GetUser(_chatRepository);
        //    var model = new MessageTypingModelView(user, id);
        //    var result = RenderViewToString(ControllerContext, "_MessageTyping", model);
        //    ChatHub.Typing(roomId, id, result, stopTyping);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void AddRoom(Room model)
        {
            var id = _chatRepository.AddRoom(model);
            model.Id = id;
            var result = ChatController.RenderViewToString(ControllerContext, "_Room", model);
            ChatHub.AddRoom(result);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebar()
        {
            var user = _accountProvider.GetUser(_chatRepository);
            RoomsModelView model = new RoomsModelView(_chatRepository, user);
            return PartialView("_Sidebar", model);
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}