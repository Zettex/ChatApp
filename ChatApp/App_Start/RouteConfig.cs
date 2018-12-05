using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChatApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                "Login",
                "Login",
                new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                "Register",
                "Register",
                new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
               "Logout",
               "Logout",
               new { controller = "Account", action = "Logout" }
            );

            routes.MapRoute(
               "Room",
               "Room/{id}",
               new { controller = "Chat", action = "Room", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               "User",
               "User/{id}",
               new { controller = "Account", action = "User", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               "ManageRooms",
               "ManageRooms",
               new { controller = "Account", action = "ManageRooms" }
            );

            routes.MapRoute(
               "RenameRoom",
               "RenameRoom",
               new { controller = "Account", action = "RenameRoom" }
            );

            routes.MapRoute(
               "DeleteRoom",
               "DeleteRoom/{id}",
               new { controller = "Account", action = "DeleteRoom", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "ManageUsers",
               "ManageUsers",
               new { controller = "Account", action = "ManageUsers" }
            );

            routes.MapRoute(
               "DeleteUser",
               "DeleteUser/{id}",
               new { controller = "Account", action = "DeleteUser", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               "DeleteMessage",
               "DeleteMessage",
               new { controller = "Account", action = "DeleteMessage" }
            );

            routes.MapRoute(
               "GetMessage",
               "GetMessage",
               new { controller = "Chat", action = "GetMessage" }
           );

            routes.MapRoute(
               "Default",
               "{action}", 
               new { controller = "Chat", action = "Room" }
           );
        }
    }
}
