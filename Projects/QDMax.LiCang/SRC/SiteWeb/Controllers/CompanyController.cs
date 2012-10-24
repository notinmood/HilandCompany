using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HiLand.Project.SiteWeb.Controllers
{
    public class CompanyController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("DomesticCapitalRegister");
        }

        public ActionResult DomesticCapitalRegister()
        {
            return View();
        }

        public ActionResult ForeignCapitalRegister()
        {
            return View();
        }

        public ActionResult CompanyChanges()
        {
            return View();
        }

        public ActionResult CompanyDestroy()
        {
            return View();
        }

        public ActionResult BranchRegister()
        {
            return View();
        }

        public ActionResult MarkerRegister()
        {
            return View();
        }
    }
}