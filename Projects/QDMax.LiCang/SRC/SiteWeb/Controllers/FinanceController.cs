using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HiLand.Project.SiteWeb.Controllers
{
    public class FinanceController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("FinanceManagement");
        }

        public ActionResult FinanceManagement()
        {
            return View();
        }

        public ActionResult InternalAudit()
        {
            return View();
        }

        public ActionResult ExportTaxRebate()
        {
            return View();
        }

        public ActionResult SpecialAudit()
        {
            return View();
        }
    }
}
