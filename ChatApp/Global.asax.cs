using ChatApp.Controllers;
using ChatApp.Providers;
using ChatApp.Repositories;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ChatApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IChatRepository>().To<ChatRepository>();
            kernel.Bind<IAccountProvider>().To<AccountProvider>();
            DependencyResolver.SetResolver(new ChatDependencyResolver(kernel));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var context = ((MvcApplication)sender).Context;
            if (context == null)
            {
                return;
            }

            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));
            var errorInfo = new HandleErrorInfo(
                Server.GetLastError(),
                routeData.Values["controller"].ToString(),
                routeData.Values["action"].ToString()
            );

            var action = "Index";
            var controller = new ErrorController();

            if (errorInfo.Exception != null)
            {
                if (errorInfo.Exception is HttpException)
                {
                    var ex = (HttpException)errorInfo.Exception;
                    switch (ex.GetHttpCode())
                    {
                        case 404:
                            context.Response.StatusCode = 404;
                            action = "NotFound";
                            break;
                    }
                }
            }

            context.ClearError();
            context.Response.Clear();
            context.Response.StatusCode = 500;
            context.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = errorInfo;
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }
    }

    public class ChatDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public ChatDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType, new IParameter[0]);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType, new IParameter[0]);
        }
    }
}
