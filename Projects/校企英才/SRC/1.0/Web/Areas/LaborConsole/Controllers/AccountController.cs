using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore.BLL;
using XQYC.Business.BLL;
using XQYC.Business.Entity;

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
            LaborEntity entity = LaborEntity.Empty;

            entity = LaborBLL.Instance.Get(BusinessUserBLL.CurrentUserGuid);

            return View(entity);
        }

        public ActionResult MyBasicInfoEdit()
        {
            return View();
        }
    }
}
