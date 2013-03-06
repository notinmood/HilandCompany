using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XQYC.Web.Areas.LaborConsole.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 用户自己修改口令
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 个人登录信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyProfile()
        {
            return View();
        }

        /// <summary>
        /// 个人基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyBasicInfo()
        {
            return View();
        }
    }
}
