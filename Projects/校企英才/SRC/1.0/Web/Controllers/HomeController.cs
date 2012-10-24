using System.Collections.Specialized;
using System.Web.Mvc;
using HiLand.Framework4.Permission.Attributes;
using HiLand.Utility.Enums;

namespace XQYC.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 仪表盘
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize(PermissionAuthorizeModes.LoginedAsPass)]
        [UserAuthorize(UserTypes.SuperAdmin | UserTypes.Manager)]
        public ActionResult Index()
        {
            ViewBag.Message = "欢迎使用校企英才数据库管理系统!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult OnlyTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OnlyTest(bool isOnlyPlaceHolder = true)
        {
            NameValueCollection nvc = this.Request.Form;
            int i = 0;
            i++;
            return View();
        }

        public ActionResult ControlTest()
        {
            return View();
        }

        public ActionResult BusinessTest()
        {
            return View();
        }
    }
}
