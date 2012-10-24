using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework4.Permission;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Cache;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Entity.Status;
using HiLand.Utility.Enums;
using HiLand.Utility4.Data;

namespace HiLand.Project.SiteWeb.Areas.Manage.Controllers
{
    [UserAuthorize]
    public class MainController : Controller
    {
        //
        // GET: /Manage/Main/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SoftVersion()
        {
            Dictionary<string, string> softVersionDic = new Dictionary<string, string>();

            Assembly assemblyWeb = Assembly.GetExecutingAssembly();
            softVersionDic.Add("运行网站", GetAssemblyInfo(assemblyWeb));

            Assembly assemblyFramework = Assembly.GetAssembly(typeof(BusinessUser));
            softVersionDic.Add("系统框架2", GetAssemblyInfo(assemblyFramework));

            Assembly assemblyFramework4 = Assembly.GetAssembly(typeof(PermissionValidationHelper));
            softVersionDic.Add("系统框架4", GetAssemblyInfo(assemblyFramework));

            Assembly assemblyGeneral = Assembly.GetAssembly(typeof(LoanBasicEntity));
            softVersionDic.Add("组件通用业务", GetAssemblyInfo(assemblyGeneral));

            Assembly assemblyUtility = Assembly.GetAssembly(typeof(AssemblyHelper));
            softVersionDic.Add("组件Utility2", GetAssemblyInfo(assemblyUtility));

            Assembly assemblyUtility4 = Assembly.GetAssembly(typeof(StringEx));
            softVersionDic.Add("组件Utility4", GetAssemblyInfo(assemblyUtility4));

            return View(softVersionDic);
        }

        [NonAction]
        private string GetAssemblyInfo(Assembly assembly)
        {
            Version version = assembly.GetName().Version;
            return string.Format("版本号为：{0}.{1}.{2}.{3}<br/>最后编译时间为:{4}", version.Major, version.Minor, version.Build, version.Revision, AssemblyHelper.GetCompiledTime(assembly).ToString("yyyy-MM-dd"));
        }


        public ActionResult CacheInfo()
        {
            ICache cache = CacheFactory.Create();
            return View(cache.Count);
        }

        [HttpPost]
        public ActionResult CacheInfo(bool isOnlyPlaceHolder = true)
        {
            ICache cache = CacheFactory.Create();
            cache.Clear();
            return View(cache.Count);
        }

        public ActionResult BasicSettingList()
        {
            List<BasicSettingEntity> basicSettingList = BasicSettingBLL.Instance.GetList(string.Empty);
            return View(basicSettingList);
        }

        public ActionResult BasicSetting(int settingID)
        {
            BasicSettingEntity basicSettingEntity = BasicSettingEntity.Empty;
            if (settingID > 0)
            {
                basicSettingEntity = BasicSettingBLL.Instance.Get(settingID);
            }

            return View(basicSettingEntity);
        }

        [HttpPost]
        public ActionResult BasicSetting(int settingID, string SettingValue)
        {
            LogicStatusInfo statusInfo = new LogicStatusInfo();
            BasicSettingEntity basicSettingEntity = BasicSettingEntity.Empty;
            if (settingID > 0)
            {
                basicSettingEntity = BasicSettingBLL.Instance.Get(settingID);
                basicSettingEntity.SettingValue = SettingValue;
                statusInfo.IsSuccessful = BasicSettingBLL.Instance.Update(basicSettingEntity);
            }

            return Json(statusInfo);
        }

        /// <summary>
        /// 用户操作结果展示
        /// </summary>
        /// <param name="isSuccessful"></param>
        /// <param name="displayInformation"></param>
        /// <returns></returns>
        public ActionResult OperationResult(bool isSuccessful, string displayInformation)
        {
            SystemStatusInfo statusInfo = new SystemStatusInfo();
            if (isSuccessful == true)
            {
                statusInfo.SystemStatus = SystemStatuses.Success;
            }
            else
            {
                statusInfo.SystemStatus = SystemStatuses.Failuer;
            }

            statusInfo.Message = displayInformation;
            return View(statusInfo);
        }
    }
}
