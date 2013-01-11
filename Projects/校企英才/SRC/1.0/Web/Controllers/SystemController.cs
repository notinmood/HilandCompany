using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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
using HiLand.Utility.Event;
using HiLand.Utility.IO;
using HiLand.Utility.Paging;
using HiLand.Utility.Web;
using HiLand.Utility4.Data;
using HiLand.Utility4.MVC;
using HiLand.Utility4.MVC.Controls;
using Webdiyer.WebControls.Mvc;
using XQYC.Business.BLL;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    public class SystemController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NoPermissionDisplayPage()
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

        [PermissionAuthorize]
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

        [PermissionAuthorize]
        public ActionResult BasicSettingList()
        {
            List<BasicSettingEntity> basicSettingList = BasicSettingBLL.Instance.GetList(string.Empty);
            return View(basicSettingList);
        }

        [PermissionAuthorize]
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
        public ActionResult BasicSetting(int settingID, BasicSettingEntity settingInfo)
        {
            LogicStatusInfo statusInfo = new LogicStatusInfo();
            BasicSettingEntity basicSettingEntity = BasicSettingEntity.Empty;
            if (settingID > 0)
            {
                basicSettingEntity = BasicSettingBLL.Instance.Get(settingID);
                basicSettingEntity.SettingValue = settingInfo.SettingValue;
                basicSettingEntity.OrderNumber = settingInfo.OrderNumber;
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
        public ActionResult OperationResult(bool isSuccessful, string displayInformation, string returnUrl = StringHelper.Empty, bool isUsingCompress = false)
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
            return RedirectToAction("OperationResults", new { returnUrl = returnUrl, isUsingCompress = isUsingCompress });
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
        public ActionResult OperationResults(string returnUrl = StringHelper.Empty, bool isUsingCompress = false)
        {
            List<SystemStatusInfo> statusInfos = new List<SystemStatusInfo>();
            object tempData = this.TempData["OperationResultData"];
            if (tempData != null)
            {
                statusInfos = (List<SystemStatusInfo>)tempData;
            }

            this.ViewData["returnUrl"] = returnUrl;
            this.ViewData["isUsingCompress"] = isUsingCompress;
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

        /// <summary>
        /// 工作移交
        /// </summary>
        /// <returns></returns>
        [PermissionAuthorize]
        public ActionResult TransferJobs()
        {
            return View();
        }

        //TODO:xieran20121123 工作移交时，考虑发送系统通知给每个内部员工
        /// <summary>
        /// 工作移交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize]
        public ActionResult TransferJobs(bool onlyPlaceHolder = true)
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
            string returnUrl = RequestHelper.CurrentFullUrl;

            Guid sourceUserGuid = ControlHelper.GetRealValue<Guid>("SourceUser");
            string sourceUserName = RequestHelper.GetValue("SourceUser");

            Guid targetUserGuid = ControlHelper.GetRealValue<Guid>("TargetUser");
            string targetUserName = RequestHelper.GetValue("TargetUser");

            Guid enterpriseGuid = ControlHelper.GetRealValue<Guid>("EnterpriseName");

            if (sourceUserGuid == Guid.Empty || targetUserGuid == Guid.Empty)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = string.Format("请先选择移交人和被移交人信息，谢谢。");
                infoList.Add(itemError);
            }
            else
            {
                #region 劳务人员移交
                {
                    int rowCount = UpdateLaborData(sourceUserGuid, targetUserGuid, targetUserName, enterpriseGuid);

                    if (rowCount > 0)
                    {
                        SystemStatusInfo itemError = new SystemStatusInfo();
                        itemError.SystemStatus = SystemStatuses.Success;
                        itemError.Message = string.Format("劳务人员数据移交成功！共移交{0}笔数据。", rowCount);
                        infoList.Add(itemError);
                    }
                    else
                    {
                        SystemStatusInfo itemError = new SystemStatusInfo();
                        itemError.SystemStatus = SystemStatuses.Tip;
                        itemError.Message = string.Format("{0}尚无劳务人员数据需要移交。", sourceUserName);
                        infoList.Add(itemError);
                    }
                }
                #endregion

                #region 企业移交
                {
                    int rowCount = UpdateEnterpriseData(sourceUserGuid, targetUserGuid, targetUserName, enterpriseGuid);

                    if (rowCount > 0)
                    {
                        SystemStatusInfo itemError = new SystemStatusInfo();
                        itemError.SystemStatus = SystemStatuses.Success;
                        itemError.Message = string.Format("企业数据移交成功！共移交{0}笔数据。", rowCount);
                        infoList.Add(itemError);
                    }
                    else
                    {
                        SystemStatusInfo itemError = new SystemStatusInfo();
                        itemError.SystemStatus = SystemStatuses.Tip;
                        itemError.Message = string.Format("{0}尚无企业数据需要移交。", sourceUserName);
                        infoList.Add(itemError);
                    }
                }
                #endregion

                #region 信息员移交
                {
                    int rowCount = UpdateInformationBrokerData(sourceUserGuid, targetUserGuid, targetUserName);

                    if (rowCount > 0)
                    {
                        SystemStatusInfo itemError = new SystemStatusInfo();
                        itemError.SystemStatus = SystemStatuses.Success;
                        itemError.Message = string.Format("信息员数据移交成功！共移交{0}笔数据。", rowCount);
                        infoList.Add(itemError);
                    }
                    else
                    {
                        SystemStatusInfo itemError = new SystemStatusInfo();
                        itemError.SystemStatus = SystemStatuses.Tip;
                        itemError.Message = string.Format("{0}尚无信息员数据需要移交。", sourceUserName);
                        infoList.Add(itemError);
                    }
                }
                #endregion
            }

            this.TempData.Add("OperationResultData", infoList);
            return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
        }

        private static int UpdateInformationBrokerData(Guid sourceUserGuid, Guid targetUserGuid, string targetUserName)
        {
            string businessUser = RequestHelper.GetValue("cbxInformationBrokerBusinessUser").ToLower();
            string serviceUser = RequestHelper.GetValue("cbxInformationBrokerServiceUser").ToLower();
            string settleUser = RequestHelper.GetValue("cbxInformationBrokerSettleUser").ToLower();
            string providerUser = RequestHelper.GetValue("cbxInformationBrokerProviderUser").ToLower();
            string recommendUser = RequestHelper.GetValue("cbxInformationBrokerRecommendUser").ToLower();
            string financeUser = RequestHelper.GetValue("cbxInformationBrokerFinanceUser").ToLower();

            int rowCountForSettleUser = 0;
            int rowCountForServiceUser = 0;
            int rowCountForBusinessUser = 0;
            int rowCountForProviderUser = 0;
            int rowCountForRecommendUser = 0;
            int rowCountForFinanceUser = 0;

            if (businessUser == "on")
            {
                string sqlClauseForLaborBusinessUser = string.Format("UPDATE [XQYCInformationBroker] SET [BusinessUserGuid] = '{0}',[BusinessUserName] = '{1}' WHERE [BusinessUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                rowCountForBusinessUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborBusinessUser);
            }


            if (serviceUser == "on")
            {
                string sqlClauseForLaborServiceUser = string.Format("UPDATE [XQYCInformationBroker] SET [ServiceUserGuid] = '{0}',[ServiceUserName] = '{1}' WHERE [ServiceUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                rowCountForServiceUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborServiceUser);
            }

            if (serviceUser == "on")
            {
                string sqlClauseForLaborProviderUser = string.Format("UPDATE [XQYCInformationBroker] SET [ProviderUserGuid] = '{0}',[ProviderUserName] = '{1}' WHERE [ProviderUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                rowCountForProviderUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborProviderUser);
            }

            if (serviceUser == "on")
            {
                string sqlClauseForLaborRecommendUser = string.Format("UPDATE [XQYCInformationBroker] SET [RecommendUserGuid] = '{0}',[RecommendUserName] = '{1}' WHERE [RecommendUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                rowCountForRecommendUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborRecommendUser);
            }

            if (serviceUser == "on")
            {
                string sqlClauseForLaborFinanceUser = string.Format("UPDATE [XQYCInformationBroker] SET [FinanceUserGuid] = '{0}',[FinanceUserName] = '{1}' WHERE [FinanceUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                rowCountForFinanceUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborFinanceUser);
            }

            if (serviceUser == "on")
            {
                string sqlClauseForLaborSettleUser = string.Format("UPDATE [XQYCInformationBroker] SET [SettleUserGuid] = '{0}',[SettleUserName] = '{1}' WHERE [SettleUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                rowCountForSettleUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborSettleUser);
            }

            int rowCountForLabor = rowCountForBusinessUser + rowCountForServiceUser + rowCountForProviderUser +
                rowCountForRecommendUser + rowCountForFinanceUser + rowCountForSettleUser;
            return rowCountForLabor;
        }

        private static int UpdateEnterpriseData(Guid sourceUserGuid, Guid targetUserGuid, string targetUserName, Guid enterpriseGuid)
        {
            string manageUser = RequestHelper.GetValue("cbxEnterpriseManageUser").ToLower();
            string businessUser = RequestHelper.GetValue("cbxLaborBusinessUser").ToLower();
            string serviceUser = RequestHelper.GetValue("cbxLaborServiceUser").ToLower();
            string settleUser = RequestHelper.GetValue("cbxLaborSettleUser").ToLower();
            string providerUser = RequestHelper.GetValue("cbxLaborProviderUser").ToLower();
            string recommendUser = RequestHelper.GetValue("cbxLaborRecommendUser").ToLower();
            string financeUser = RequestHelper.GetValue("cbxLaborFinanceUser").ToLower();

            int rowCountForManageUser = 0;
            int rowCountForSettleUser = 0;
            int rowCountForServiceUser = 0;
            int rowCountForBusinessUser = 0;
            int rowCountForProviderUser = 0;
            int rowCountForRecommendUser = 0;
            int rowCountForFinanceUser = 0;

            if (manageUser == "on")
            {
                string sqlClauseForLaborManageUser = string.Format("UPDATE [GeneralEnterprise] SET [ManageUserKey] = '{0}',[ManageUserName] = '{1}' WHERE [ManageUserKey]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborManageUser += string.Format(" AND EnterpriseGuid='{0}' ", enterpriseGuid);
                }
                rowCountForManageUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborManageUser);
            }

            if (businessUser == "on")
            {
                string sqlClauseForLaborBusinessUser = string.Format("UPDATE [XQYCEnterpriseService] SET [BusinessUserGuid] = '{0}',[BusinessUserName] = '{1}' WHERE [BusinessUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborBusinessUser += string.Format(" AND EnterpriseGuid='{0}' ", enterpriseGuid);
                }
                rowCountForBusinessUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborBusinessUser);
            }

            if (serviceUser == "on")
            {
                string sqlClauseForLaborServiceUser = string.Format("UPDATE [XQYCEnterpriseService] SET [ServiceUserGuid] = '{0}',[ServiceUserName] = '{1}' WHERE [ServiceUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborServiceUser += string.Format(" AND EnterpriseGuid='{0}' ", enterpriseGuid);
                }
                rowCountForServiceUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborServiceUser);
            }

            if (providerUser == "on")
            {
                string sqlClauseForLaborProviderUser = string.Format("UPDATE [XQYCEnterpriseService] SET [ProviderUserGuid] = '{0}',[ProviderUserName] = '{1}' WHERE [ProviderUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborProviderUser += string.Format(" AND EnterpriseGuid='{0}' ", enterpriseGuid);
                }
                rowCountForProviderUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborProviderUser);
            }

            if (recommendUser == "on")
            {
                string sqlClauseForLaborRecommendUser = string.Format("UPDATE [XQYCEnterpriseService] SET [RecommendUserGuid] = '{0}',[RecommendUserName] = '{1}' WHERE [RecommendUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborRecommendUser += string.Format(" AND EnterpriseGuid='{0}' ", enterpriseGuid);
                }
                rowCountForRecommendUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborRecommendUser);
            }

            if (financeUser == "on")
            {
                string sqlClauseForLaborFinanceUser = string.Format("UPDATE [XQYCEnterpriseService] SET [FinanceUserGuid] = '{0}',[FinanceUserName] = '{1}' WHERE [FinanceUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborFinanceUser += string.Format(" AND EnterpriseGuid='{0}' ", enterpriseGuid);
                }
                rowCountForFinanceUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborFinanceUser);
            }

            if (settleUser == "on")
            {
                string sqlClauseForLaborSettleUser = string.Format("UPDATE [XQYCEnterpriseService] SET [SettleUserGuid] = '{0}',[SettleUserName] = '{1}' WHERE [SettleUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborSettleUser += string.Format(" AND EnterpriseGuid='{0}' ", enterpriseGuid);
                }
                rowCountForSettleUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborSettleUser);
            }

            int rowCountForLabor = rowCountForManageUser + rowCountForBusinessUser + rowCountForServiceUser + rowCountForProviderUser +
                rowCountForRecommendUser + rowCountForFinanceUser + rowCountForSettleUser;
            return rowCountForLabor;
        }

        private static int UpdateLaborData(Guid sourceUserGuid, Guid targetUserGuid, string targetUserName, Guid enterpriseGuid)
        {
            string businessUser = RequestHelper.GetValue("cbxLaborBusinessUser").ToLower();
            string serviceUser = RequestHelper.GetValue("cbxLaborServiceUser").ToLower();
            string settleUser = RequestHelper.GetValue("cbxLaborSettleUser").ToLower();
            string providerUser = RequestHelper.GetValue("cbxLaborProviderUser").ToLower();
            string recommendUser = RequestHelper.GetValue("cbxLaborRecommendUser").ToLower();
            string financeUser = RequestHelper.GetValue("cbxLaborFinanceUser").ToLower();

            int rowCountForSettleUser = 0;
            int rowCountForServiceUser = 0;
            int rowCountForBusinessUser = 0;
            int rowCountForProviderUser = 0;
            int rowCountForRecommendUser = 0;
            int rowCountForFinanceUser = 0;

            if (businessUser == "on")
            {
                string sqlClauseForLaborBusinessUser = string.Format("UPDATE [XQYCLabor] SET [BusinessUserGuid] = '{0}',[BusinessUserName] = '{1}' WHERE [BusinessUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborBusinessUser += string.Format(" AND CurrentEnterpriseKey='{0}' ", enterpriseGuid);
                }

                rowCountForBusinessUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborBusinessUser);
            }

            if (serviceUser == "on")
            {
                string sqlClauseForLaborServiceUser = string.Format("UPDATE [XQYCLabor] SET [ServiceUserGuid] = '{0}',[ServiceUserName] = '{1}' WHERE [ServiceUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborServiceUser += string.Format(" AND CurrentEnterpriseKey='{0}' ", enterpriseGuid);
                }

                rowCountForServiceUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborServiceUser);
            }

            if (providerUser == "on")
            {
                string sqlClauseForLaborProviderUser = string.Format("UPDATE [XQYCLabor] SET [ProviderUserGuid] = '{0}',[ProviderUserName] = '{1}' WHERE [ProviderUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborProviderUser += string.Format(" AND CurrentEnterpriseKey='{0}' ", enterpriseGuid);
                }
                rowCountForProviderUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborProviderUser);
            }

            if (recommendUser == "on")
            {
                string sqlClauseForLaborRecommendUser = string.Format("UPDATE [XQYCLabor] SET [RecommendUserGuid] = '{0}',[RecommendUserName] = '{1}' WHERE [RecommendUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborRecommendUser += string.Format(" AND CurrentEnterpriseKey='{0}' ", enterpriseGuid);
                }
                rowCountForRecommendUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborRecommendUser);
            }

            if (financeUser == "on")
            {
                string sqlClauseForLaborFinanceUser = string.Format("UPDATE [XQYCLabor] SET [FinanceUserGuid] = '{0}',[FinanceUserName] = '{1}' WHERE [FinanceUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborFinanceUser += string.Format(" AND CurrentEnterpriseKey='{0}' ", enterpriseGuid);
                }
                rowCountForFinanceUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborFinanceUser);
            }

            if (settleUser == "on")
            {
                string sqlClauseForLaborSettleUser = string.Format("UPDATE [XQYCLabor] SET [SettleUserGuid] = '{0}',[SettleUserName] = '{1}' WHERE [SettleUserGuid]= '{2}' ", targetUserGuid, targetUserName, sourceUserGuid);
                if (enterpriseGuid != Guid.Empty)
                {
                    sqlClauseForLaborSettleUser += string.Format(" AND CurrentEnterpriseKey='{0}' ", enterpriseGuid);
                }
                rowCountForSettleUser = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForLaborSettleUser);
            }

            int rowCountForLabor = rowCountForBusinessUser + rowCountForServiceUser + rowCountForProviderUser +
                rowCountForRecommendUser + rowCountForFinanceUser + rowCountForSettleUser;
            return rowCountForLabor;
        }

        public ActionResult OperateLogList(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("OperateLogList", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";
            string orderClause = "LogID DESC";
            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            PagedEntityCollection<OperateLogEntity> entityList = OperateLogBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<OperateLogEntity> pagedExList = new PagedList<OperateLogEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }
    }
}
