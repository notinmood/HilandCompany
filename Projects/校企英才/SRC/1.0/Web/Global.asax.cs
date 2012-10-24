using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HiLand.Utility.Setting;
using Quartz;
using Quartz.Impl;
using XQYC.Web.Models;

namespace XQYC.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        IScheduler scheduler = null;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new string[] { "XQYC.Web.Controllers" }
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //debug模式下不启动系统自动运行的任务
            bool isDebugMode = Config.GetAppSetting<bool>("debugMode");
            if (isDebugMode == false)
            {
                ISchedulerFactory factory = new StdSchedulerFactory();
                scheduler = factory.GetScheduler();
                JobContainer.ExecuteJobs(scheduler);
                scheduler.Start();
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //DataBaseLoger.SaveExcuteInfo("Other", "App quit");
            if (scheduler != null)
            {
                scheduler.Shutdown();
            }
        }
    }
}