using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework4.Permission.Attributes;
using HiLand.Utility.Enums;

namespace XQYC.Web.Areas.InformationBrokerConsole.Controllers
{
    [UserAuthorize(UserTypes.Broker)]
    public class HomeController : Controller
    {
        //
        // GET: /InformationBroker/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
