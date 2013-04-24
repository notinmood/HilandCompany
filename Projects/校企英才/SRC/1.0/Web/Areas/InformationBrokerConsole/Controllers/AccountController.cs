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
        /// 个人基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyProfile()
        {
            return View();
        }
    }
}
