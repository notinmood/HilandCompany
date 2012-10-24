using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HilandSoftSiteWeb.Controllers
{
    /// <summary>
    /// 物联网应用
    /// </summary>
    public class IOTController : Controller
    {
        /// <summary>
        /// 关于物联网
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 智能酒店
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartHotel()
        {
            return View();
        }

        
        /// <summary>
        /// 第四代港口物流
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartPort()
        {
            return View();
        }
    }
}
