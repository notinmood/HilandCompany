using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Enums;
using XQYC.Business.BLL;
using XQYC.Business.Entity;

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

            //获取个人提醒信息
            List<RemindEntity> remindList= RemindBLL.Instance.GetListUnRead(BusinessUserBLL.CurrentUserGuid,8);
            this.ViewData["remindList"] = remindList;

            //获取个人提醒信息的数量
            int remindCount = RemindBLL.Instance.GetCountUnRead(BusinessUserBLL.CurrentUserGuid);
            this.ViewData["remindCount"] = remindCount;

            //获取最新录入的劳务人员
            List<LaborEntity> laborLastestList= LaborBLL.Instance.GetListForLastest(8);
            this.ViewData["laborLastestList"] = laborLastestList;

            //获取最新录入的劳务人员数量
            int laborRegisterCount = LaborBLL.Instance.GetCountForLastestRegister(DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1));
            this.ViewData["laborRegisterCount"] = laborRegisterCount;

            //获取最新离职的劳务人员
            List<LaborContractEntity> laborDiscontinueContractList = LaborContractBLL.Instance.GetListForLastestDiscontinue(8);
            this.ViewData["laborDiscontinueContractList"] = laborDiscontinueContractList;

            //获取最新离职的劳务人员数量
            int laborDiscontinueCount = LaborContractBLL.Instance.GetCountForLastestDiscontinue(DateTime.Today.AddMonths(-1),DateTime.Today.AddDays(1));
            this.ViewData["laborDiscontinueCount"] = laborDiscontinueCount;

            //获取最新入职的劳务人员
            List<LaborContractEntity> laborLastContractList = LaborContractBLL.Instance.GetListForLastestContract(8);
            this.ViewData["laborLastContractList"] = laborLastContractList;

            //获取最新入职的劳务人员数量
            int laborLastCountCount = LaborContractBLL.Instance.GetCountForLastestContract(DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1));
            this.ViewData["laborLastCountCount"] = laborLastCountCount;

            //获取最新招工简章
            List<EnterpriseJobEntity> enterpriseJobLastList = EnterpriseJobBLL.Instance.GetListForLastest(8);
            this.ViewData["enterpriseJobLastList"] = enterpriseJobLastList;

            //获取最新招工简章数量
            int enterpriseJobCount = EnterpriseJobBLL.Instance.GetCountForLastest(DateTime.Today.AddMonths(-1), DateTime.Today.AddDays(1));
            this.ViewData["enterpriseJobCount"] = enterpriseJobCount;

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
