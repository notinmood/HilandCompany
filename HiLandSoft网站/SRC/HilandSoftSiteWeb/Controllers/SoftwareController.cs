using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HilandSoftSiteWeb.Controllers
{
    public class SoftwareController : Controller
    {
        /// <summary>
        /// 关于软件
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 全景软件
        /// </summary>
        /// <returns></returns>
        public ActionResult FullView()
        {
            return View();
        }

        /// <summary>
        /// 广告管理软件
        /// </summary>
        /// <returns></returns>
        public ActionResult AD()
        {
            return View();
        }

    }
}
