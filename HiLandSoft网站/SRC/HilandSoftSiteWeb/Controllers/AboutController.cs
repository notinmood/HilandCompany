using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HilandSoftSiteWeb.Controllers
{
    public class AboutController : Controller
    {
        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 联系我们
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// 企业文化
        /// </summary>
        /// <returns></returns>
        public ActionResult Culture()
        {
            return View();
        }

        /// <summary>
        /// 经典案例
        /// </summary>
        /// <returns></returns>
        public ActionResult Case()
        {
            return View();
        }
    }
}
