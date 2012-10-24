using System.Web.Mvc;

namespace XQYC.Web.Controllers
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
        /// 管理员为用户修改口令
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePasswordForManage()
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
