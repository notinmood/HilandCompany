using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework4.Permission.Attributes;
using HiLand.Utility.Enums;

namespace XQYC.Web.Areas.EnterpriseConsole.Controllers
{
    [UserAuthorize(UserTypes.EnterpriseUser)]
    public class EnterpriseController : Controller
    {
        //
        // GET: /EnterpriseConsole/Enterprise/

        public ActionResult Index()
        {
            return View();
        }

    }
}
