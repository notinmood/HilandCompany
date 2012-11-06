using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework4.Permission;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Cache;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Entity.Status;
using HiLand.Utility.Enums;
using HiLand.Utility.Event;
using HiLand.Utility.IO;
using HiLand.Utility.Web;
using HiLand.Utility4.Data;
using HiLand.Utility4.MVC;

namespace XQYC.Web.Controllers
{
    public class SystemController : Controller
    {
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
        public ActionResult OperationResult(bool isSuccessful, string displayInformation, string returnUrl = StringHelper.Empty)
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
            List<SystemStatusInfo> statusInfos = new List<SystemStatusInfo>();
            statusInfos.Add(statusInfo);

            this.TempData.Add("OperationResultData", statusInfos);
            return RedirectToAction("OperationResults", new { returnUrl = returnUrl });
        }

        /// <summary>
        /// 用户操作结果展示(请一定注意本方法的备注说明)
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 需要展示的状态信息列表请必须是List<SystemStatusInfo>类型，并且这个数据传递过来的时候，
        /// 1、需要通过TempData的方法来传递
        /// 2、传递时的key必须为“OperationResultData”
        /// 如下：
        ///     List<SystemStatusInfo> statusInfos = new List<SystemStatusInfo>();
        ///     this.TempData.Add("OperationResultData", statusInfos);
        /// </remarks>
        public ActionResult OperationResults(string returnUrl = StringHelper.Empty)
        {
            List<SystemStatusInfo> statusInfos = new List<SystemStatusInfo>();
            object tempData = this.TempData["OperationResultData"];
            if (tempData != null)
            {
                statusInfos = (List<SystemStatusInfo>)tempData;
            }

            this.ViewData["returnUrl"] = returnUrl;
            return View("OperationResult", statusInfos);
        }

        /// <summary>
        /// 信息导出另存
        /// </summary>
        /// <param name="isOnlyPlaceHolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OperationResults(bool isOnlyPlaceHolder = true)
        {
            int inputDisplayCount = RequestHelper.GetValue("inputDisplayCount", 0);

            if (inputDisplayCount > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < inputDisplayCount; i++)
                {
                    string inputDisplayContent = RequestHelper.GetValue("inputDisplayContent+" + i);
                    sb.AppendLine(inputDisplayContent);
                }

                Stream stream = new MemoryStream(StringHelper.GetByteArray(sb.ToString()));
                stream.Flush();

                return File(stream, ContentTypes.GetContentType(".txt"), "操作信息.txt");
            }

            return new EmptyResult();
        }

        /// <summary>
        /// 常用模板下载
        /// </summary>
        /// <returns></returns>
        public ActionResult TempletList()
        {
            return View();
        }
    }
}
