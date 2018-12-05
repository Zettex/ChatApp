using ChatApp.Hubs;
using ChatApp.Models;
using ChatApp.Models.AccountModels;
using ChatApp.Providers;
using ChatApp.Repositories;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IChatRepository _chatRepository;

        public AccountController(IAccountProvider authProvider, IChatRepository chatRepository = null)
        {
            _accountProvider = authProvider;
            _chatRepository = chatRepository;
        }

        public ActionResult Login(string returnUrl)
        {
            if (_accountProvider.IsLoggedIn)
                return RedirectToAction("Room", "Chat");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && 
                _accountProvider.Login(_chatRepository, model.Name, model.Password))
            {
                return RedirectToAction("Room", "Chat");
            }

            ModelState.AddModelError("", "Invalid login or password");
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (_accountProvider.IsLoggedIn)
                return RedirectToAction("Room", "Chat");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _chatRepository.GetUser(0, model.Name);
                if (user != null)
                {
                    ModelState.AddModelError("", "A user with this name is already in the system");
                    return View(model);
                }

                // TODO: make submit application instead add to users
                user = new User {
                    Name = model.Name,
                    Password = model.Password,
                    Role = "User",
                    CanCreateRoom = true,
                    CanWriteMessages = true
                };

                _chatRepository.AddUser(user);
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        [Authorize]
        public new ActionResult User(int id = 0)
        {
            if (id != 0)
            {
                var user = _chatRepository.GetUser(id);
                if (user == null)
                    throw new HttpException(404, "HTTP/1.1 404 Not Found");
                var currentUser = _accountProvider.GetUser(_chatRepository);
                UserModelView model = new UserModelView(user, currentUser);
                return View(model);
            }

            // User.ImageUrl make request to server? and get 404..
            throw new HttpException(404, "HTTP/1.1 404 Not Found");
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public new ActionResult User(User user)
        {
            var editingUser = _chatRepository.GetUser(user.Id);

            if (ModelState.IsValid)
            {

                if (editingUser.Id != _chatRepository.GetUser(0, user.Name).Id)
                    ModelState.AddModelError("", "This name is already taken");
                else
                    _chatRepository.EditUser(user);
            }

            var currentUser = _accountProvider.GetUser(_chatRepository);
            UserModelView model = new UserModelView(editingUser, currentUser);

            return View(model);
        }

        [Authorize]
        public ActionResult ManageRooms()
        {
            if (_accountProvider.GetUser(_chatRepository).Role == "User")
                return RedirectToAction("Room", "Chat");

            var rooms = _chatRepository.GetRooms();
            return View(rooms);
        }

        [HttpPost]
        [Authorize]
        public ActionResult RenameRoom(Room room)
        {
            _chatRepository.EditRoom(room);
            return RedirectToAction("ManageRooms", "Account");
        }

        [Authorize]
        public ActionResult DeleteRoom(int id)
        {
            // Warning: When deleting a room, in the signalr groups will
            // be broken. After deleting the room, all users must be called /Logout.
            var room = _chatRepository.GetRoom(id);
            if (room == null)
                throw new HttpException(500, "HTTP/1.1 500 Internal Server Error");

            if (_accountProvider.GetUser(_chatRepository).Role != "User")
            {
                _chatRepository.DeleteRoom(room);
            }

            return RedirectToAction("ManageRooms", "Account");
        }

        [Authorize]
        public ActionResult ManageUsers()
        {
            if (_accountProvider.GetUser(_chatRepository).Role == "User")
                return RedirectToAction("Room", "Chat");

            var users = _chatRepository.GetUsers();
            return View(users);
        }

        [Authorize]
        public ActionResult DeleteUser(int id)
        {
            var user = _chatRepository.GetUser(id);
            if (user == null)
                throw new HttpException(500, "HTTP/1.1 500 Internal Server Error");

            if (_accountProvider.GetUser(_chatRepository).Role != "User")
            {
                if (user.Role != "SuperAdmin")
                    _chatRepository.DeleteUser(user);
            }

            var users = _chatRepository.GetUsers();
            return RedirectToAction("ManageUsers", "Account");
        }

        [HttpPost]
        [Authorize]
        public void DeleteMessage(int messageId, string roomId)
        {
            _chatRepository.DeleteMessage(messageId);
            ChatHub.DeleteMessage(messageId.ToString(), roomId);
        }

        public ActionResult Logout()
        {
            if (_accountProvider.IsLoggedIn)
                _accountProvider.Logout();

            return RedirectToAction("Login", "Account");
        }
    }
}