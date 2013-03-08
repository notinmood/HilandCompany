using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.Membership;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;

namespace XQYC.Web.Areas.UserCenter.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

                switch (loginStatus)
                {
                    case LoginStatuses.Successful:
                        logicStatusInfo.IsSuccessful = true;
                        break;
                    case LoginStatuses.FailureUnactive:
                        logicStatusInfo.IsSuccessful = false;
                        return RedirectToAction("Active", "Home", new { area = "UserCenter" });
                    default:
                        logicStatusInfo.IsSuccessful = false;
                        break;
                }
                //if (loginStatus == LoginStatuses.Successful)
                //{
                //    logicStatusInfo.IsSuccessful = true;
                //}
                //else
                //{
                //    logicStatusInfo.IsSuccessful = false;
                //}

                logicStatusInfo.Message = EnumHelper.GetDisplayValue(loginStatus);
            }

            if (logicStatusInfo.IsSuccessful == true)
            {
                if (FormsAuthentication.GetRedirectUrl(userName, false) == FormsAuthentication.DefaultUrl)
                {
                    switch (businessUser.UserType)
                    {
                        case UserTypes.EnterpriseUser:
                            return RedirectToAction("Index", "Home", new { area = "EnterpriseConsole" });
                        case UserTypes.Broker:
                            return RedirectToAction("Index", "Home", new { area = "InformationBrokerConsole" });
                        case UserTypes.Manager:
                        case UserTypes.SuperAdmin:
                            return RedirectToAction("Index", "Home", new { area = "" });
                        case UserTypes.CommonUser:
                        default:
                            return RedirectToAction("Index", "Home", new { area = "LaborConsole" });
                    }
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(userName, false);
                }
            }

            return View(logicStatusInfo);
        }

        /// <summary>
        /// 网站等第三方登录系统
        /// </summary>
        /// <param name="u"></param>
        /// <param name="p"></param>
        /// <param name="isBase64">账号口令是否经过了base64转码</param>
        /// <returns></returns>
        public ActionResult WebIndex(string u, string p, bool isBase64 = true)
        {
            if (isBase64 == true && string.IsNullOrWhiteSpace(u) == false && string.IsNullOrWhiteSpace(p) == false)
            {
                byte[] uArray = Convert.FromBase64String(u);
                u = Encoding.UTF8.GetString(uArray);

                byte[] pArray = Convert.FromBase64String(p);
                p = Encoding.UTF8.GetString(pArray);
            }

            return Index(u, p);
        }


        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            UserCookie userCookie = UserCookie.Load<UserCookie>();
            userCookie.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _ChangePasswordForManage(string userGuid, string userName = "", string returnUrl = StringHelper.Empty)
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
        public ActionResult _ChangePasswordForManage(string userGuid, string passwordNew, string passwordNewConfirm, bool isOnlyPlaceHolder = true)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            logicStatusInfo.IsSuccessful = BusinessUserBLL.ChangePassword(new Guid(userGuid), passwordNew);

            return Json(logicStatusInfo);
        }



        public ActionResult _ChangePasswordForSelf()
        {
            PassCurrentUser();
            return View();
        }

        [HttpPost]
        public ActionResult _ChangePasswordForSelf(Guid userGuid, string passwordOld, string passwordNew, string passwordNewConfirm)
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
        public ActionResult _MyProfile()
        {
            return View();
        }

        public ActionResult Active()
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
