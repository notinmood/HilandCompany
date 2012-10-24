using System;
using System.Web.Mvc;
using System.Web.Security;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.Membership;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;

namespace HiLand.Project.SiteWeb.Areas.Manage.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string password)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            LoginStatuses loginStatus = LoginStatuses.Successful;
            BusinessUser businessUser = null;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                logicStatusInfo.IsSuccessful = false;
                logicStatusInfo.Message = "请必须输入账号和口令，谢谢！";
            }
            else
            {
                businessUser = BusinessUserBLL.Login(userName, password, out loginStatus);

                if (loginStatus == LoginStatuses.Successful)
                {
                    logicStatusInfo.IsSuccessful = true;
                }
                else
                {
                    logicStatusInfo.IsSuccessful = false;
                    logicStatusInfo.Message = loginStatus.ToString();
                }
            }

            if (logicStatusInfo.IsSuccessful == true)
            {
                FormsAuthentication.RedirectFromLoginPage(userName, false);
            }

            return View(logicStatusInfo);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            UserCookie userCookie = UserCookie.Load<UserCookie>();
            userCookie.Clear();
            return RedirectToAction("Index", "Account", new { area = "Manage" });
        }

        public ActionResult UChangePassword(string userGuid, string userName = "", string returnUrl = StringHelper.Empty)
        {
            BusinessUser businessUser = BusinessUserBLL.Get(userGuid);
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = businessUser.UserName;
            }
            this.ViewBag.ReturnUrl = returnUrl;
            return View(businessUser);
        }

        [HttpPost]
        public ActionResult UChangePassword(string userGuid, string passwordNew, string passwordNewConfirm, bool isOnlyPlaceHolder = true)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            logicStatusInfo.IsSuccessful = BusinessUserBLL.ChangePassword(new Guid(userGuid), passwordNew);

            return Json(logicStatusInfo);
        }

        public ActionResult ChangePassword()
        {
            PassCurrentUser();
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(Guid userGuid, string passwordOld, string passwordNew, string passwordNewConfirm)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            logicStatusInfo.IsSuccessful = BusinessUserBLL.ChangePassword(userGuid, passwordNew, passwordOld);

            PassCurrentUser();
            return View(logicStatusInfo);
        }

        /// <summary>
        /// 个人基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyProfile()
        {
            return View();
        }

        #region 辅助方法
        private void PassCurrentUser()
        {
            string currentUserName = string.Empty;
            bool isAuthenticated = this.Request.RequestContext.HttpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated == true)
            {
                currentUserName = this.Request.RequestContext.HttpContext.User.Identity.Name;
            }

            if (string.IsNullOrWhiteSpace(currentUserName) == true)
            {
                UserCookie userCookie = UserCookie.Load<UserCookie>();
                currentUserName = userCookie.UserName;
            }

            BusinessUser currentUser = BusinessUserBLL.Get(currentUserName);
            this.ViewBag.CurrentUser = currentUser;
        }
        #endregion
    }
}
