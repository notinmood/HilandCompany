using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using XQYC.Web.Areas.APIs.Models;

namespace XQYC.Web.Areas.APIs.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Help()
        {
            var explorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();
            return View(new ApiModel(explorer)); 
        }

    }
}
