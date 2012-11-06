using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.BusinessCore.Enum;
using HiLand.Framework4.Permission;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Entity.Status;
using HiLand.Utility.Enums;
using HiLand.Utility.IO;
using HiLand.Utility.Office;
using HiLand.Utility.Paging;
using HiLand.Utility.Reflection;
using HiLand.Utility.UI;
using HiLand.Utility.Web;
using HiLand.Utility4.MVC.Controls;
using NPOI.POIFS.FileSystem;
using Webdiyer.WebControls.Mvc;
using XQYC.Business.BLL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    [PermissionAuthorize]
    public class LaborController : Controller
    {
        #region 基本信息
        public ActionResult Index(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("Index", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("LaborQuery", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";

            //--数据权限----------------------------------------------------------------------
            whereClause += " AND ( ";
            whereClause += string.Format(" {0} ", PermissionDataHelper.GetFilterCondition("FinanceUserGuid"));
            whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("ProviderUserGuid"));
            whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("RecommendUserGuid"));
            whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("ServiceUserGuid"));
            whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("BusinessUserGuid"));
            whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("SettleUserGuid"));
            whereClause += " ) ";
            //--end--------------------------------------------------------------------------

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("LaborQuery");

            string orderClause = "LaborID DESC";

            PagedEntityCollection<LaborEntity> entityList = LaborBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<LaborEntity> pagedExList = new PagedList<LaborEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            bool isExportExcel = RequestHelper.GetValue("exportExcel", false);
            if (isExportExcel == true)
            {
                return LaborListToExcelFile(entityList.Records);
            }
            else
            {
                return View(pagedExList);
            }
        }

        private ActionResult LaborListToExcelFile(IList<LaborEntity> laborList)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["UserNameCN"] = "人员名称";
            dic["UserCardID"] = "身份证号码";
            dic["UserStatus"] = "人员状态";
            dic["LaborWorkStatus"] = "工作状态";
            dic["CurrentEnterpriseName"] = "务工企业";
            dic["LaborCode"] = "职工编号";
            dic["UserSex"] = "性别";
            dic["UserAge"] = "年龄";
            dic["UserBirthDay"] = "出生日期";
            dic["UserMobileNO"] = "联系电话";
            dic["UrgentTelephone"] = "紧急联系电话";
            dic["CurrentContractStartDate"] = "最近合同开始时间";
            dic["CurrentContractStopDate"] = "最近合同到期时间";
            dic["CurrentContractDiscontinueDate"] = "最近离职时间";
            dic["ProviderUserName"] = "信息提供人员";
            dic["RecommendUserName"] = "推荐人员";
            dic["FinanceUserName"] = "财务人员";
            dic["ServiceUserName"] = "客服人员";

            Stream excelStream = ExcelHelper.WriteExcel(laborList, dic);
            return File(excelStream, ContentTypes.GetContentType("xls"), "劳务人员信息.xls");
        }

        public ActionResult Item(string itemKey)
        {
            LaborEntity entity = GetLabor(itemKey);
            return View(entity);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Item(string itemKey, LaborEntity entity, bool isOnlyPlaceHolder = true)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            CreateUserRoleStatuses createStatus = CreateUserRoleStatuses.Successful;

            string returnUrl = RequestHelper.GetValue("returnUrl");
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index");
            }

            LaborEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey))
            {
                targetEntity = new LaborEntity();
                SetTargetEntityValue(entity, ref targetEntity);
                targetEntity.UserRegisterDate = DateTime.Now;
                targetEntity.UserType = UserTypes.CommonUser;
                targetEntity.Password = SystemConst.InitialUserPassword;
                targetEntity.LaborWorkStatus = LaborWorkStatuses.NewWorker;
                createStatus = LaborBLL.Instance.Create(targetEntity);
                if (createStatus == CreateUserRoleStatuses.Successful)
                {
                    isSuccessful = true;
                }
            }
            else
            {
                targetEntity = LaborBLL.Instance.Get(itemKey);
                SetTargetEntityValue(entity, ref targetEntity);

                isSuccessful = LaborBLL.Instance.Update(targetEntity);
                if (isSuccessful == false)
                {
                    createStatus = CreateUserRoleStatuses.FailureUnknowReason;
                }
            }

            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功";
            }
            else
            {
                displayMessage = string.Format("数据保存失败,原因为{0}", EnumHelper.GetDisplayValue(createStatus));
            }

            return Redirect(returnUrl);
        }

        /// <summary>
        /// 通过一个实体给另外一个实体赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        private static void SetTargetEntityValue(LaborEntity originalEntity, ref LaborEntity targetEntity)
        {
            targetEntity.UserStatus = originalEntity.UserStatus;
            targetEntity.UserNameCN = originalEntity.UserNameCN;
            targetEntity.UserSex = originalEntity.UserSex;
            targetEntity.UserBirthDay = originalEntity.UserBirthDay;

            targetEntity.UserCardID = originalEntity.UserCardID;
            targetEntity.IDCardPlace = originalEntity.IDCardPlace;
            targetEntity.UserHeight = originalEntity.UserHeight;
            targetEntity.UserEducationalBackground = originalEntity.UserEducationalBackground;
            targetEntity.UserNation = originalEntity.UserNation;
            targetEntity.NativePlace = originalEntity.NativePlace;
            targetEntity.UserMobileNO = originalEntity.UserMobileNO;
            targetEntity.HomeTelephone = originalEntity.HomeTelephone;
            targetEntity.WorkSkill = originalEntity.WorkSkill;
            targetEntity.WorkSkillPaper = originalEntity.WorkSkillPaper;
            targetEntity.WorkSituation = originalEntity.WorkSituation;
            targetEntity.PreWorkSituation = originalEntity.PreWorkSituation;
            targetEntity.HopeWorkSituation = originalEntity.HopeWorkSituation;
            targetEntity.HopeWorkSalary = originalEntity.HopeWorkSalary;
            targetEntity.MaritalStatus = originalEntity.MaritalStatus;
            targetEntity.UrgentLinkMan = originalEntity.UrgentLinkMan;
            targetEntity.UrgentTelephone = originalEntity.UrgentTelephone;
            targetEntity.UrgentRelationship = originalEntity.UrgentRelationship;
            targetEntity.Memo1 = originalEntity.Memo1;
            targetEntity.Memo2 = originalEntity.Memo2;
            targetEntity.Memo3 = originalEntity.Memo3;
            targetEntity.Memo4 = originalEntity.Memo4;
            targetEntity.Memo5 = originalEntity.Memo5;

            targetEntity.InformationBrokerUserGuid = ControlHelper.GetRealValue<Guid>("InformationBroker");
            targetEntity.InformationBrokerUserName = RequestHelper.GetValue("InformationBroker");
            targetEntity.FinanceUserName = RequestHelper.GetValue("FinanceUser");
            targetEntity.FinanceUserGuid = RequestHelper.GetValue<Guid>("FinanceUser_Value");
            targetEntity.ProviderUserName = RequestHelper.GetValue("ProviderUser");
            targetEntity.ProviderUserGuid = RequestHelper.GetValue<Guid>("ProviderUser_Value");
            targetEntity.RecommendUserName = RequestHelper.GetValue("RecommendUser");
            targetEntity.RecommendUserGuid = RequestHelper.GetValue<Guid>("RecommendUser_Value");
            targetEntity.ServiceUserName = RequestHelper.GetValue("ServiceUser");
            targetEntity.ServiceUserGuid = RequestHelper.GetValue<Guid>("ServiceUser_Value");
            targetEntity.BusinessUserName = RequestHelper.GetValue("BusinessUser");
            targetEntity.BusinessUserGuid = RequestHelper.GetValue<Guid>("BusinessUser_Value");
            targetEntity.SettleUserName = RequestHelper.GetValue("SettleUser");
            targetEntity.SettleUserGuid = RequestHelper.GetValue<Guid>("SettleUser_Value");
        }

        /// <summary>
        /// 劳务人员批量录入
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchEntering()
        {
            return View();
        }

        /// <summary>
        /// 劳务人员批量录入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchEntering(bool isOnlyPlaceholder = true)
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
            string returnUrl = RequestHelper.CurrentFullUrl;

            Guid providerUserGuid = ControlHelper.GetRealValue<Guid>("ProviderUser");
            string providerUserName = RequestHelper.GetValue("ProviderUser");
            Guid recommendUserGuid = ControlHelper.GetRealValue<Guid>("RecommendUser");
            string recommendUserName = RequestHelper.GetValue("RecommendUser");
            Guid financeUserGuid = ControlHelper.GetRealValue<Guid>("FinanceUser");
            string financeUserName = RequestHelper.GetValue("FinanceUser");
            Guid serviceUserGuid = ControlHelper.GetRealValue<Guid>("ServiceUser");
            string serviceUserName = RequestHelper.GetValue("ServiceUser");


            Guid informationBrokerUserGuid = ControlHelper.GetRealValue<Guid>("InformationBroker");
            string informationBrokerUserName = RequestHelper.GetValue("InformationBroker");
            //Guid informationBrokerUserGuid = RequestHelper.GetValue<Guid>("InformationBrokerUserGuid");
            //string informationBrokerUserName = RequestHelper.GetValue("InformationBrokerUserGuid_Text");

            int headerRowNumber = RequestHelper.GetValue<int>("headerRowNumber", 1);


            HttpPostedFile postedFile = RequestHelper.CurrentRequest.Files["fileSelector"];
            if (HttpPostedFileHelper.HasFile(postedFile))
            {
                try
                {
                    int userCountSuccessful = 0;
                    int userCountFailure = 0;
                    string userListFailure = string.Empty;

                    DataTable dataTable = ExcelHelper.ReadExcel(postedFile.InputStream, headerRowNumber);
                    NameValueCollection laborMapData = (NameValueCollection)ConfigurationManager.GetSection("laborBasicDataMap");

                    List<string> columnNameList = new List<string>();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        columnNameList.Add(dataTable.Columns[i].ColumnName);
                    }

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        int originalExcelRowNumber = headerRowNumber + i + 1;
                        try
                        {
                            DataRow row = dataTable.Rows[i];
                            if (row == null)
                            {
                                continue;
                            }

                            string userAgeString = string.Empty;
                            string userBirhdayString = string.Empty;
                            string userSexString = string.Empty;

                            LaborEntity laborEntity = new LaborEntity();
                            foreach (string columnName in columnNameList)
                            {
                                //1.根据Excel文件中的列名称映射Labor实体的属性名称
                                string propertyName = laborMapData[columnName];
                                if (string.IsNullOrWhiteSpace(propertyName))
                                {
                                    continue;
                                }

                                //2.给Labor实体属性赋值
                                object cellValue = row[columnName];
                                if (cellValue != null)
                                {
                                    switch (propertyName)
                                    {
                                        case "HouseHoldType":
                                            laborEntity.HouseHoldType = EnumHelper.GetItem<HouseHoldTypes>(cellValue.ToString());
                                            break;
                                        case "MaritalStatus":
                                            laborEntity.MaritalStatus = EnumHelper.GetItem<MaritalStatuses>(cellValue.ToString());
                                            break;
                                        case "UserSex":
                                            userSexString = cellValue.ToString();
                                            break;
                                        case "UserEducationalBackground":
                                            laborEntity.UserEducationalBackground = EnumHelper.GetItem<EducationalBackgrounds>(cellValue.ToString());
                                            break;
                                        case "UserAge":
                                            userAgeString = cellValue.ToString();
                                            break;
                                        case "UserBirthday":
                                            userBirhdayString = cellValue.ToString();
                                            break;
                                        default:
                                            ReflectHelper.SetPropertyValue<LaborEntity>(laborEntity, propertyName, cellValue);
                                            break;
                                    }
                                }
                            }

                            laborEntity.Password = SystemConst.InitialUserPassword;
                            laborEntity.UserType = UserTypes.CommonUser;
                            laborEntity.UserRegisterDate = DateTime.Now;

                            laborEntity.ProviderUserGuid = providerUserGuid;
                            laborEntity.ProviderUserName = providerUserName;
                            laborEntity.RecommendUserGuid = recommendUserGuid;
                            laborEntity.RecommendUserName = recommendUserName;
                            laborEntity.FinanceUserGuid = financeUserGuid;
                            laborEntity.FinanceUserName = financeUserName;
                            laborEntity.ServiceUserGuid = serviceUserGuid;
                            laborEntity.ServiceUserName = serviceUserName;
                            laborEntity.InformationBrokerUserGuid = informationBrokerUserGuid;
                            laborEntity.InformationBrokerUserName = informationBrokerUserName;

                            IDCard idCard = IDCard.Parse(laborEntity.UserCardID);
                            //人员生日的抽取先后顺序1、直接的生日输入 2、身份证中提取 3、年龄计算。（满足前面的条件，自动跳过后面的条件）
                            DateTime userBirthDay = DateTimeHelper.Parse(userBirhdayString, DateFormats.YMD);
                            if (userBirthDay == DateTimeHelper.Min)
                            {
                                userBirthDay = idCard.BirthDay;
                            }
                            if (userBirthDay == DateTimeHelper.Min)
                            {
                                userBirthDay = new DateTime(DateTime.Today.Year - Converter.ChangeType(userAgeString, 25), 1, 1);
                            }
                            laborEntity.UserBirthDay = userBirthDay;

                            //人员性别的抽取顺序1、直接输入的性别 2、身份证中提取
                            Sexes userSex = EnumHelper.GetItem<Sexes>(userSexString, string.Empty, Sexes.UnSet);
                            if (userSex == Sexes.UnSet)
                            {
                                userSex = idCard.Sex;
                            }
                            laborEntity.UserSex = userSex;

                            //必须人员姓名和身份证同时为空时，不保存
                            if (string.IsNullOrWhiteSpace(laborEntity.UserNameCN) && string.IsNullOrWhiteSpace(laborEntity.UserCardID))
                            {
                                userCountFailure++;
                                userListFailure += string.Format("{0}(人员姓名和身份证同时为空),<br/> ", originalExcelRowNumber);
                            }
                            else
                            {
                                CreateUserRoleStatuses createStatus = LaborBLL.Instance.Create(laborEntity);

                                if (createStatus == CreateUserRoleStatuses.Successful)
                                {
                                    userCountSuccessful++;
                                }
                                else
                                {
                                    userCountFailure++;
                                    userListFailure += string.Format("{0}({1}),<br/> ", originalExcelRowNumber, EnumHelper.GetDisplayValue(createStatus));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            userCountFailure++;
                            userListFailure += originalExcelRowNumber + ", ";
                        }
                    }

                    SystemStatusInfo itemSuccessful = new SystemStatusInfo();
                    itemSuccessful.SystemStatus = SystemStatuses.Success;
                    itemSuccessful.Message = string.Format("共有{0}人导入成功。", userCountSuccessful);
                    infoList.Add(itemSuccessful);

                    if (userCountFailure > 0)
                    {
                        SystemStatusInfo itemError = new SystemStatusInfo();
                        itemError.SystemStatus = SystemStatuses.Failuer;
                        itemError.Message = string.Format("共有{0}人导入失败。", userCountFailure);
                        if (string.IsNullOrWhiteSpace(userListFailure) == false)
                        {
                            itemError.Message += string.Format("导入失败的人员分别位于{0}行", userListFailure);
                        }
                        infoList.Add(itemError);
                    }
                }
                catch (NPOI.POIFS.FileSystem.OfficeXmlFileException)
                {
                    SystemStatusInfo itemError = new SystemStatusInfo();
                    itemError.SystemStatus = SystemStatuses.Failuer;
                    itemError.Message = "请选择Excel2003格式的文件。你可以将本文件在Excel中另存的时候选择97/2003格式！";
                    infoList.Add(itemError);
                }
            }
            else
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "没有选择Excel文件，请先选择文件然后再进行导入！";
                infoList.Add(itemError);
            }

            this.TempData.Add("OperationResultData", infoList);
            return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
        }

        /// <summary>
        /// 口令
        /// </summary>
        /// <param name="itemKey"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public ActionResult Password(string itemKey, string itemName = "")
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                itemName = EmployeeBLL.Instance.Get(itemKey).UserName;
            }

            string returnUrl = RequestHelper.CurrentRequest.AppRelativeCurrentExecutionFilePath;
            return RedirectToAction("_ChangePasswordForManage", "Home", new { area = "UserCenter", userGuid = itemKey, userName = itemName, returenUrl = returnUrl });
        }

        /// <summary>
        /// 劳务人员自动完成的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult LaborAutoCompleteData()
        {
            string userValueInputted = RequestHelper.GetValue("term");
            string enterpriserGuid = RequestHelper.GetValue("autoCompleteExtraParam");
            List<AutoCompleteEntity> itemList = new List<AutoCompleteEntity>();
            string whereClause = string.Format(" UserNameCN like '{0}%' AND UserType={1} ", userValueInputted, (int)UserTypes.CommonUser);

            //为了能够为刚刚离职的人员派发薪资，此处不再使用按照企业过滤劳务人员数据
            //if (string.IsNullOrWhiteSpace(enterpriserGuid) == false)
            //{
            //    whereClause += string.Format(" AND CurrentEnterpriseKey='{0}' ", enterpriserGuid);
            //}

            List<LaborEntity> userList = LaborBLL.Instance.GetList(whereClause);

            foreach (LaborEntity currentLabor in userList)
            {
                string inCurrentEnterpriseDesc = string.Empty;
                if (string.IsNullOrWhiteSpace(enterpriserGuid) == false
                    && string.IsNullOrWhiteSpace(currentLabor.CurrentEnterpriseKey) == false
                    && enterpriserGuid.ToLower() == currentLabor.CurrentEnterpriseKey.ToLower())
                {
                    inCurrentEnterpriseDesc = "在职";
                }
                else
                {
                    inCurrentEnterpriseDesc = "其他";
                }

                AutoCompleteEntity item = new AutoCompleteEntity();
                item.details = "nothing";
                item.key = currentLabor.UserGuid.ToString();
                item.label = string.Format("{0}({1}({2}))", currentLabor.UserNameCN, currentLabor.UserCardID, inCurrentEnterpriseDesc);
                item.value = currentLabor.UserNameCN;

                itemList.Add(item);
            }

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        private LaborEntity GetLabor(string itemKey)
        {
            LaborEntity entity = LaborEntity.Empty;
            if (string.IsNullOrWhiteSpace(itemKey) == false)
            {
                entity = LaborBLL.Instance.Get(itemKey);
            }

            return entity;
        }
        #endregion

        #region 合同
        public ActionResult ContractList(string itemKey)
        {
            List<LaborContractEntity> userList = new List<LaborContractEntity>();

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format("LaborUserGuid='{0}'", itemGuid.ToString());
                string orderbyClause = string.Format("LaborContractIsCurrent DESC,LaborContractID DESC");
                userList = LaborContractBLL.Instance.GetList(whereClause, orderbyClause);
            }

            this.ViewBag.ItemKey = itemKey;
            return View(userList);
        }

        public ActionResult ContractItem(string userKey, string itemKey = StringHelper.Empty)
        {
            LaborContractEntity entity = LaborContractEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                entity = LaborContractBLL.Instance.Get(itemKey);
            }

            entity.LaborUserGuid = GuidHelper.TryConvert(userKey);
            return View(entity);
        }

        [HttpPost]
        public ActionResult ContractItem(string itemKey, LaborContractEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            LaborContractEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetEntity = new LaborContractEntity();
                targetEntity.LaborUserGuid = RequestHelper.GetValue<Guid>("UserKey");
                targetEntity.OperateDate = DateTime.Now;
                targetEntity.OperateUserGuid = BusinessUserBLL.CurrentUser.UserGuid;

                SetTargetContractEntityValue(originalEntity, ref  targetEntity);

                isSuccessful = LaborContractBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = LaborContractBLL.Instance.Get(itemKey);

                SetTargetContractEntityValue(originalEntity, ref  targetEntity);
                isSuccessful = LaborContractBLL.Instance.Update(targetEntity);
            }

            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功";
            }
            else
            {
                displayMessage = "数据保存失败";
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }

        private void SetTargetContractEntityValue(LaborContractEntity originalEntity, ref LaborContractEntity targetEntity)
        {
            targetEntity.EnterpriseGuid = ControlHelper.GetRealValue<Guid>("EnterpriseName");
            targetEntity.LaborContractDetails = originalEntity.LaborContractDetails;
            targetEntity.EnterpriseContractGuid = originalEntity.EnterpriseContractGuid;
            targetEntity.LaborContractStartDate = originalEntity.LaborContractStartDate;
            targetEntity.LaborContractStopDate = originalEntity.LaborContractStopDate;
            targetEntity.LaborContractStatus = originalEntity.LaborContractStatus;

            targetEntity.LaborCode = originalEntity.LaborCode;
            targetEntity.LaborContractIsCurrent = originalEntity.LaborContractIsCurrent;

            targetEntity.EnterpriseInsuranceFormularKey = originalEntity.EnterpriseInsuranceFormularKey;
            targetEntity.EnterpriseManageFeeFormularKey = originalEntity.EnterpriseManageFeeFormularKey;
            targetEntity.EnterpriseMixCostFormularKey = originalEntity.EnterpriseMixCostFormularKey;
            targetEntity.EnterpriseOtherCostFormularKey = originalEntity.EnterpriseOtherCostFormularKey;
            targetEntity.EnterpriseReserveFundFormularKey = originalEntity.EnterpriseReserveFundFormularKey;

            targetEntity.PersonInsuranceFormularKey = originalEntity.PersonInsuranceFormularKey;
            targetEntity.PersonManageFeeFormularKey = originalEntity.PersonManageFeeFormularKey;
            targetEntity.PersonMixCostFormularKey = originalEntity.PersonMixCostFormularKey;
            targetEntity.PersonOtherCostFormularKey = originalEntity.PersonOtherCostFormularKey;
            targetEntity.PersonReserveFundFormularKey = originalEntity.PersonReserveFundFormularKey;

            targetEntity.LaborContractDiscontinueDate = originalEntity.LaborContractDiscontinueDate;
            targetEntity.LaborContractDiscontinueDesc = originalEntity.LaborContractDiscontinueDesc;
        }
        #endregion

        #region 工资管理
        /// <summary>
        /// 薪资银行报盘预选取器
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryToBank()
        {
            return View();
        }

        /// <summary>
        /// 薪资银行报盘预选取器
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryToBank(bool isOnlyPlaceHolder = true)
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
            string returnUrl = RequestHelper.CurrentFullUrl;

            string enterpriseGuid = ControlHelper.GetRealValue("EnterpriseName", string.Empty);
            EnterpriseEntity enterpriseEntity = EnterpriseEntity.Empty;
            string salaryMonth = RequestHelper.GetValue("salaryMonth");
            DateTime salaryDate = DateTimeHelper.Min;

            if (string.IsNullOrWhiteSpace(salaryMonth))
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "你没有选定薪资月份，请选择。";
                infoList.Add(itemError);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                salaryMonth = salaryMonth + "/1";
                salaryDate = DateTimeHelper.Parse(salaryMonth, DateFormats.YMD);
            }

            if (GuidHelper.IsInvalidOrEmpty(enterpriseGuid))
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "你没有选定企业的名称信息，请选择。";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                enterpriseEntity = EnterpriseBLL.Instance.Get(enterpriseGuid);
            }

            DataTable salaryTable = GenerateSalaryDataTabel(enterpriseEntity, salaryDate);
            Stream salaryStream = ExcelHelper.WriteExcel(salaryTable, false);

            string fileDownloadName = string.Format("{0}-{1}", enterpriseEntity.CompanyName, salaryDate.ToString("yyyyMM"));
            return File(salaryStream, ContentTypes.GetContentType("xls"), fileDownloadName);
        }

        /// <summary>
        /// 生成某企业某薪资月份的薪资报盘数据
        /// </summary>
        /// <param name="enterpriseGuid"></param>
        /// <param name="salaryMonth"></param>
        /// <returns></returns>
        private static DataTable GenerateSalaryDataTabel(EnterpriseEntity enterprise, DateTime salaryMonth)
        {
            DataTable salaryTable = new DataTable();

            //1.创建表头
            DataColumn BankCardNumber = new DataColumn("BankCardNumber", typeof(String));
            salaryTable.Columns.Add(BankCardNumber);
            DataColumn columnUserNameCN = new DataColumn("UserNameCN", typeof(String));
            salaryTable.Columns.Add(columnUserNameCN);
            DataColumn SalaryValue = new DataColumn("SalaryValue", typeof(decimal));
            salaryTable.Columns.Add(SalaryValue);
            DataColumn SalaryMemo = new DataColumn("SalaryMemo", typeof(String));
            salaryTable.Columns.Add(SalaryMemo);

            //2.添加表数据
            List<SalarySummaryEntity> salarySummaryList = SalarySummaryBLL.Instance.GetList(enterprise.EnterpriseGuid.ToString(), salaryMonth);
            foreach (SalarySummaryEntity item in salarySummaryList)
            {
                DataRow row = salaryTable.NewRow();
                //获取某用户银行首要账户
                row[BankCardNumber] = BankBLL.Instance.GetPrimary(Converter.TryToGuid(item.LaborKey)).AccountNumber;
                row[columnUserNameCN] = item.LaborName;
                row[SalaryValue] = item.SalaryNeedPayToLabor;
                row[SalaryMemo] = string.Format("{0}-{1}-工资", enterprise.CompanyNameShort, salaryMonth.ToString("yyyyMM"));
                salaryTable.Rows.Add(row);
            }

            return salaryTable;
        }

        /// <summary>
        /// 薪资信息查询预选取器
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryListPreSelector()
        {
            return View();
        }

        /// <summary>
        /// 薪资信息查询预选取器
        /// </summary>
        /// <param name="isOnlyPlaceHolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryListPreSelector(bool isOnlyPlaceHolder = true)
        {
            string enterpriseKey = ControlHelper.GetRealValue("EnterpriseName", string.Empty);
            string salaryMonth = RequestHelper.GetValue("salaryMonth");
            return RedirectToActionPermanent("SalaryList", new { enterpriseKey = enterpriseKey, salaryMonth = salaryMonth });
        }

        public ActionResult SalaryList(int id = 1)
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
            string returnUrl = Url.Action("SalaryListPreSelector");

            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";
            string orderClause = "SalarySummaryID DESC";

            //1.加入对预选择条件的过滤
            string enterpriseKey = RequestHelper.GetValue("enterpriseKey");
            string salaryMonth = RequestHelper.GetValue("salaryMonth");
            if (string.IsNullOrWhiteSpace(enterpriseKey))
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "你没有选定企业信息，请选择。";
                infoList.Add(itemError);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                whereClause += string.Format(" AND EnterpriseKey='{0}' ", enterpriseKey);
            }

            if (string.IsNullOrWhiteSpace(salaryMonth))
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "你没有选定薪资月份，请选择。";
                infoList.Add(itemError);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                salaryMonth = HttpUtility.UrlDecode(salaryMonth);
                string salaryDateString = salaryMonth + "/1";
                DateTime salaryDateFirstDay = DateTimeHelper.Parse(salaryDateString, DateFormats.YMD);
                DateTime salaryDateLastDay = DateTimeHelper.GetFirstDateOfMonth(salaryDateFirstDay.AddMonths(1));
                whereClause += string.Format(" AND SalaryDate>='{0}' AND SalaryDate<'{1}' ", salaryDateFirstDay, salaryDateLastDay);
            }

            //2、加入对查询控件选择条件的过滤
            //2.1、如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("SalaryList", new { id = 1, enterpriseKey = enterpriseKey, salaryMonth = salaryMonth });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("LaborQuery", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }


            //2.2、通常情形下走get查询
            whereClause += " AND " + QueryControlHelper.GetQueryCondition("LaborQuery");
            PagedEntityCollection<SalarySummaryEntity> entityList = SalarySummaryBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<SalarySummaryEntity> pagedExList = new PagedList<SalarySummaryEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }

        /// <summary>
        /// 添加某人的工资Summary
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryItem()
        {
            string enterpriseKey = RequestHelper.GetValue("enterpriseKey");
            EnterpriseEntity enterpriseEntity = EnterpriseEntity.Empty;
            if (string.IsNullOrWhiteSpace(enterpriseKey) == false)
            {
                enterpriseEntity = EnterpriseBLL.Instance.Get(enterpriseKey);
            }
            string salaryMonth = RequestHelper.GetValue("salaryMonth");
            this.ViewData["salaryMonth"] = salaryMonth;

            return View(enterpriseEntity);
        }

        /// <summary>
        /// 添加某人的工资Summary
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryItem(bool isOnlyPlaceHolder = true)
        {
            string returnUrl = RequestHelper.CurrentFullUrl;
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();

            string enterpriseKey = RequestHelper.GetValue("enterpriseGuid");
            Guid userGuid = RequestHelper.GetValue<Guid>("UserNameCN_Value");
            string salaryMonth = RequestHelper.GetValue("salaryMonth");
            DateTime salaryMonthDate = DateTimeHelper.Min;
            LaborEntity labor = LaborEntity.Empty;

            #region 数据验证
            if (enterpriseKey == String.Empty)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = "无法确定企业信息";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }

            if (userGuid == Guid.Empty)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = "请先输入并选择人员信息";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                labor = LaborBLL.Instance.Get(userGuid);
            }

            if (salaryMonth == string.Empty)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = "请先输入薪资月份信息";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                salaryMonthDate = DateTimeHelper.Parse(salaryMonth + "/1", DateFormats.YMD);
            }
            #endregion

            if (salaryMonthDate == DateTimeHelper.Min)
            {
                salaryMonthDate = DateTimeHelper.GetFirstDateOfMonth(DateTime.Today);
            }

            //判断某人某月是否已经有薪资记录。1、如果有就直接使用，2、如果没有就创建新的薪资数据;(以此保证人员某月薪资数据的唯一)
            bool isSuccessful = true;
            SalarySummaryEntity salarySummaryEntity = SalarySummaryBLL.Instance.Get(labor.UserGuid.ToString(), salaryMonthDate);

            if (salarySummaryEntity.IsEmpty)
            {
                salarySummaryEntity.SalarySummaryGuid = GuidHelper.NewGuid();
                salarySummaryEntity.CreateDate = DateTime.Now;
                salarySummaryEntity.CreateUserKey = BusinessUserBLL.CurrentUserGuid.ToString();
                salarySummaryEntity.EnterpriseKey = enterpriseKey;
                salarySummaryEntity.SalaryDate = salaryMonthDate;
                salarySummaryEntity.LaborCode = labor.LaborCode;
                salarySummaryEntity.LaborKey = labor.UserGuid.ToString();
                salarySummaryEntity.LaborName = labor.UserNameCN;

                isSuccessful = SalarySummaryBLL.Instance.Create(salarySummaryEntity);
            }

            if (isSuccessful == true)
            {
                return RedirectToAction("SalaryDetailsList", new { itemKey = salarySummaryEntity.SalarySummaryGuid, enterpriseKey = enterpriseKey });
            }
            else
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = "未知错误";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
        }

        /// <summary>
        /// 工资项列表
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public ActionResult SalaryDetailsList(string itemKey)
        {
            string whereClause = string.Format(" SalarySummaryKey ='{0}' ", itemKey);
            List<SalaryDetailsEntity> salaryDetailsList = SalaryDetailsBLL.Instance.GetList(whereClause);
            return View(salaryDetailsList);
        }

        /// <summary>
        /// 具体的工资项
        /// </summary>
        /// <param name="itemkey"></param>
        /// <returns></returns>
        public ActionResult SalaryDetails(string itemkey, string salarySummaryKey = StringHelper.Empty)
        {
            SalaryDetailsEntity entity = SalaryDetailsEntity.Empty;
            entity.SalarySummaryKey = salarySummaryKey;
            if (GuidHelper.IsInvalidOrEmpty(itemkey) == false)
            {
                entity = SalaryDetailsBLL.Instance.Get(itemkey);
            }

            return View(entity);
        }

        /// <summary>
        /// 具体的工资项修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryDetails(SalaryDetailsEntity entity)
        {
            bool isSuccessful = false;
            SalaryDetailsEntity orignalEntity = SalaryDetailsEntity.Empty;
            if (entity.SalaryDetailsGuid == Guid.Empty)
            {
                isSuccessful = SalaryDetailsBLL.Instance.Create(entity);
            }
            else
            {
                orignalEntity = SalaryDetailsBLL.Instance.Get(entity.SalaryDetailsGuid, true);
                isSuccessful = SalaryDetailsBLL.Instance.Update(entity);
            }

            //同时需要修改salarySummary里面对应项的值
            if (isSuccessful == true && string.IsNullOrWhiteSpace(entity.SalarySummaryKey) == false)
            {
                SalarySummaryEntity salarySummaryEntity = SalarySummaryBLL.Instance.Get(entity.SalarySummaryKey);

                decimal itemValueDelta = entity.SalaryItemValue - orignalEntity.SalaryItemValue;
                switch (entity.SalaryItemKind)
                {
                    case SalaryItemKinds.BasicSalary:
                    case SalaryItemKinds.Rewards:
                        salarySummaryEntity.SalaryGrossPay += itemValueDelta;
                        break;
                    case SalaryItemKinds.Rebate:
                        //确保扣费为负值
                        salarySummaryEntity.SalaryRebate += (0 - Math.Abs(itemValueDelta));
                        break;
                    case SalaryItemKinds.EnterpriseInsurance:
                        salarySummaryEntity.EnterpriseInsuranceReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.EnterpriseReserveFund:
                        salarySummaryEntity.EnterpriseReserveFundReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.EnterpriseManageFee:
                        salarySummaryEntity.EnterpriseManageFeeReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.EnterpriseOtherFee:
                        salarySummaryEntity.EnterpriseOtherCostReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.PersonalInsurance:
                        salarySummaryEntity.PersonInsuranceReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.PersonalReserveFund:
                        salarySummaryEntity.PersonReserveFundReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.PersonalOtherFee:
                        salarySummaryEntity.PersonOtherCostReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.SalaryTax:
                        //do nothing.(工资税系统自动计算，不能录入)
                        break;
                    default:
                        break;
                }

                SalarySummaryBLL.Instance.Update(salarySummaryEntity);
            }

            return RedirectToAction("SalaryDetailsList", new { itemKey = entity.SalarySummaryKey });
        }

        /// <summary>
        /// 批量导入劳务人员薪资
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryBatch()
        {
            return View();
        }

        /// <summary>
        /// 批量导入劳务人员薪资
        /// </summary>
        /// <param name="isOnlyPlaceHolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryBatch(bool isOnlyPlaceHolder = true)
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
            string returnUrl = RequestHelper.CurrentFullUrl;

            Guid enterpriseGuid = ControlHelper.GetRealValue<Guid>("EnterpriseName");
            int headerRowNumber = RequestHelper.GetValue<int>("headerRowNumber", 1);
            string salaryDateString = RequestHelper.GetValue("SalaryMonth");
            DateTime salaryDate = DateTimeHelper.Min;
            HttpPostedFile postedFile = RequestHelper.CurrentRequest.Files["fileSelector"];

            #region 条件判定（如果不满足基本条件直接跳出并提示）
            if (string.IsNullOrWhiteSpace(salaryDateString))
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "你没有选定薪资月份，请选择。";
                infoList.Add(itemError);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                salaryDateString = salaryDateString + "/1";
                salaryDate = DateTimeHelper.Parse(salaryDateString, DateFormats.YMD);
            }

            if (enterpriseGuid == Guid.Empty)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "你没有选定企业的名称信息，请选择。";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }

            if (HttpPostedFileHelper.HasFile(postedFile) == false)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "没有选择Excel文件，请先选择文件然后再进行导入！";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            #endregion

            //List<LaborEntity> laborsOfCurrentEnterprise = LaborBLL.Instance.GetLaborsByEnterprise(enterpriseGuid, LaborWorkStatuses.Worked);
            ////已经完成付款的劳务人员guid集合
            //List<Guid> laborGuidsPaid = new List<Guid>();

            try
            {
                #region 将数据读入内存
                int userCountSuccessful = 0;
                int userCountFailure = 0;
                string userListFailure = string.Empty;
                DataTable dataTable = ExcelHelper.ReadExcel(postedFile.InputStream, headerRowNumber);
                NameValueCollection laborSalaryItemMapData = (NameValueCollection)ConfigurationManager.GetSection("laborSalaryItemMap");

                List<string> columnNameList = new List<string>();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    columnNameList.Add(dataTable.Columns[i].ColumnName);
                }
                #endregion

                #region 对Excel的每行信息进行解析
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int dataRowNumberInExcel = headerRowNumber + i + 1;
                    try
                    {
                        DataRow row = dataTable.Rows[i];
                        if (row == null)
                        {
                            continue;
                        }

                        string LaborUserNameCNForSalarySummary = string.Empty;
                        string LaborUserCodeForSalarySummary = string.Empty;

                        Guid salarySummaryGuid = GuidHelper.NewGuid();
                        SalarySummaryEntity salarySummaryEntity = new SalarySummaryEntity();
                        salarySummaryEntity.SalarySummaryGuid = salarySummaryGuid;
                        salarySummaryEntity.SalaryDate = salaryDate;
                        salarySummaryEntity.CreateDate = DateTime.Today;
                        salarySummaryEntity.CreateUserKey = BusinessUserBLL.CurrentUserGuid.ToString();
                        salarySummaryEntity.EnterpriseKey = enterpriseGuid.ToString();
                        salarySummaryEntity.SalaryPayStatus = SalaryPayStatuses.PaidToOrgnization;

                        foreach (string columnName in columnNameList)
                        {
                            //1.根据Excel文件中的列名称映射Labor实体的属性名称
                            string propertyName = laborSalaryItemMapData[columnName];
                            if (string.IsNullOrWhiteSpace(propertyName))
                            {
                                propertyName = columnName;
                            }

                            //2.给SalaryDetailsEntity实体属性赋值
                            object cellValue = row[columnName];
                            if (cellValue != null)
                            {
                                SalaryDetailsEntity salaryDetailsEntity = new SalaryDetailsEntity();
                                salaryDetailsEntity.SalarySummaryKey = salarySummaryGuid.ToString();
                                salaryDetailsEntity.SalaryItemKey = columnName;
                                decimal salaryItemValue = Converter.ChangeType<decimal>(cellValue);

                                switch (propertyName)
                                {
                                    case "UserNameCN":
                                        LaborUserNameCNForSalarySummary = cellValue.ToString();
                                        break;
                                    case "LaborCode":
                                        LaborUserCodeForSalarySummary = cellValue.ToString();
                                        break;
                                    //TODO:xieran20121105 重整常规费用（Cost）
                                    //case "ManageFeeReal":
                                    //    salarySummaryEntity.ManageFeeReal = salaryItemValue;
                                    //    SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.ManageFee);
                                    //    break;
                                    //case "ReserveFundReal":
                                    //    salarySummaryEntity.ReserveFundReal = salaryItemValue;
                                    //    SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.ReserveFund);
                                    //    break;
                                    //case "InsuranceReal":
                                    //    salarySummaryEntity.InsuranceReal = salaryItemValue;
                                    //    SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.Insurance);
                                    //    break;
                                    default:
                                        if (salaryItemValue >= 0)
                                        {
                                            salarySummaryEntity.SalaryGrossPay += salaryItemValue;
                                            SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.Rewards);
                                        }
                                        else
                                        {
                                            salarySummaryEntity.SalaryRebate += salaryItemValue;
                                            SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.Rebate);
                                        }
                                        break;
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(LaborUserNameCNForSalarySummary) || string.IsNullOrWhiteSpace(LaborUserCodeForSalarySummary))
                        {
                            userCountFailure++;
                            userListFailure += string.Format("{0}({1}({2})请确认此用户的用户名称和工号均不可以为空), <br />", dataRowNumberInExcel, LaborUserNameCNForSalarySummary, LaborUserCodeForSalarySummary);
                            //物联删除掉已经插入的无效的salaryDetails数据
                            SalaryDetailsBLL.Instance.DeleteList(salarySummaryEntity.SalarySummaryGuid);
                        }
                        else
                        {
                            //根据人员姓名和工号，确认劳务人员的UserGuid
                            bool isMatchedLabor = false;
                            LaborEntity laborEntity = LaborBLL.Instance.Get(LaborUserNameCNForSalarySummary, LaborUserCodeForSalarySummary, enterpriseGuid.ToString());
                            if (laborEntity.IsEmpty)
                            {
                                isMatchedLabor = false;
                            }
                            else
                            {
                                isMatchedLabor = true;
                            }

                            if (isMatchedLabor == true)
                            {
                                bool isSuccessful = true;
                                SalarySummaryEntity salarySummaryEntityConfirm = SalarySummaryBLL.Instance.Get(laborEntity.UserGuid.ToString(), salaryDate);
                                if (salarySummaryEntityConfirm.IsEmpty)
                                {
                                    salarySummaryEntity.LaborKey = laborEntity.UserGuid.ToString();
                                    salarySummaryEntity.LaborCode = LaborUserCodeForSalarySummary;
                                    salarySummaryEntity.LaborName = LaborUserNameCNForSalarySummary;

                                    isSuccessful = SalarySummaryBLL.Instance.Create(salarySummaryEntity);
                                }
                                else
                                {
                                    salarySummaryEntityConfirm.EnterpriseInsuranceReal += salarySummaryEntity.EnterpriseInsuranceReal;
                                    salarySummaryEntityConfirm.EnterpriseManageFeeReal += salarySummaryEntity.EnterpriseManageFeeReal;
                                    salarySummaryEntityConfirm.EnterpriseReserveFundReal += salarySummaryEntity.EnterpriseReserveFundReal;
                                    salarySummaryEntityConfirm.EnterpriseOtherCostReal += salarySummaryEntity.EnterpriseOtherCostReal;
                                    salarySummaryEntityConfirm.EnterpriseMixCostReal += salarySummaryEntity.EnterpriseMixCostReal;

                                    salarySummaryEntityConfirm.PersonInsuranceReal += salarySummaryEntity.PersonInsuranceReal;
                                    salarySummaryEntityConfirm.PersonManageFeeReal += salarySummaryEntity.PersonManageFeeReal;
                                    salarySummaryEntityConfirm.PersonReserveFundReal += salarySummaryEntity.PersonReserveFundReal;
                                    salarySummaryEntityConfirm.PersonOtherCostReal += salarySummaryEntity.PersonOtherCostReal;
                                    salarySummaryEntityConfirm.PersonMixCostReal += salarySummaryEntity.PersonMixCostReal;
                                    
                                    salarySummaryEntityConfirm.SalaryGrossPay += salarySummaryEntity.SalaryGrossPay;
                                    salarySummaryEntityConfirm.SalaryRebate += salarySummaryEntity.SalaryRebate;
                                    isSuccessful = SalarySummaryBLL.Instance.Update(salarySummaryEntityConfirm);
                                }

                                if (isSuccessful == true)
                                {
                                    userCountSuccessful++;
                                }
                                else
                                {
                                    userCountFailure++;
                                    userListFailure += i + ", ";
                                }
                            }
                            else
                            {
                                userCountFailure++;
                                userListFailure += string.Format("{0}({1}({2})请确认此用户的用户名称和工号是否跟系统内的数据一致), <br />", dataRowNumberInExcel, LaborUserNameCNForSalarySummary, LaborUserCodeForSalarySummary);
                                //物联删除掉已经插入的无效的salaryDetails数据
                                SalaryDetailsBLL.Instance.DeleteList(salarySummaryEntity.SalarySummaryGuid);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        userCountFailure++;
                        userListFailure += i + ", ";
                    }
                }
                #endregion

                //#region 应付而未付的用户逻辑
                //if (laborsOfCurrentEnterprise.Count - laborGuidsPaid.Count > 0)
                //{
                //    for (int w = 0; w < laborGuidsPaid.Count; w++)
                //    {
                //        LaborEntity laborEntity = laborsOfCurrentEnterprise.Find(e => e.UserGuid == laborGuidsPaid[w]);
                //        if (laborEntity != null)
                //        {
                //            laborsOfCurrentEnterprise.Remove(laborEntity);
                //        }
                //    }

                //    for (int w = 0; w < laborsOfCurrentEnterprise.Count; w++)
                //    {
                //        LaborEntity laborEntity = laborsOfCurrentEnterprise[w];

                //        SalarySummaryEntity salarySummaryEntity = new SalarySummaryEntity();
                //        salarySummaryEntity.LaborName = laborEntity.UserNameCN;
                //        salarySummaryEntity.LaborCode = laborEntity.LaborCode;
                //        salarySummaryEntity.LaborKey = laborEntity.UserGuid.ToString();
                //        salarySummaryEntity.CreateDate = DateTime.Today;
                //        salarySummaryEntity.CreateUserKey = BusinessUserBLL.CurrentUserGuid.ToString();
                //        salarySummaryEntity.EnterpriseKey = enterpriseGuid.ToString();
                //        salarySummaryEntity.SalaryDate = salaryDate;
                //        //用待付标识，此劳务人员的薪资应付但未付
                //        salarySummaryEntity.SalaryPayStatus = SalaryPayStatuses.NeedPay;
                //        CalculateNeedCost(laborEntity, salarySummaryEntity);
                //        bool isSuccessful = SalarySummaryBLL.Instance.Create(salarySummaryEntity);
                //    }
                //}
                //#endregion

                #region 操作结果展示
                //A.1、操作结果（导入成功的人员信息）
                SystemStatusInfo itemSuccessful = new SystemStatusInfo();
                itemSuccessful.SystemStatus = SystemStatuses.Success;
                itemSuccessful.Message = string.Format("共有{0}人导入成功。", userCountSuccessful);
                infoList.Add(itemSuccessful);

                ////A.2、操作结果（需要导入数据但是未导入数据的人员信息）
                //int userCountWarnning = laborsOfCurrentEnterprise.Count;
                //if (userCountWarnning > 0)
                //{
                //    SystemStatusInfo itemError = new SystemStatusInfo();
                //    itemError.SystemStatus = SystemStatuses.Warnning;
                //    itemError.Message = string.Format("共有{0}人本次未有工资信息导入。人员分别为：", userCountWarnning);
                //    foreach (LaborEntity item in laborsOfCurrentEnterprise)
                //    {
                //        itemError.Message += string.Format("{0}({1}), <br />", item.UserNameCN, item.LaborCode);
                //    }

                //    infoList.Add(itemError);
                //}

                //A.3、操作结果（导入失败的人员信息）
                if (userCountFailure > 0)
                {
                    SystemStatusInfo itemError = new SystemStatusInfo();
                    itemError.SystemStatus = SystemStatuses.Failuer;
                    itemError.Message = string.Format("共有{0}人导入失败。", userCountFailure);
                    if (string.IsNullOrWhiteSpace(userListFailure) == false)
                    {
                        itemError.Message += string.Format("导入失败的人员分别位于{0}行, <br />", userListFailure);
                    }
                    infoList.Add(itemError);
                }
                #endregion
            }
            catch (OfficeXmlFileException)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = "请选择Excel2003格式的文件。你可以将本文件在Excel中另存的时候选择97/2003格式！";
                infoList.Add(itemError);
            }

            this.TempData.Add("OperationResultData", infoList);
            return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
        }

        /// <summary>
        /// 保存工资项明细项目（保存前指定其值和类型）
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="salaryDetailsEntity"></param>
        /// <returns></returns>
        private static bool SetAndSaveSalaryDetailsItem(decimal salaryItemValue, SalaryDetailsEntity salaryDetailsEntity, SalaryItemKinds salaryItemKind)
        {
            salaryDetailsEntity.SalaryItemValue = salaryItemValue;
            salaryDetailsEntity.SalaryItemKind = salaryItemKind;
            return SalaryDetailsBLL.Instance.Create(salaryDetailsEntity);
        }

        /// <summary>
        /// 薪资人数的应付数与实付数校验
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryListCheck()
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
            string returnUrl = Url.Action("SalaryListPreSelector");

            //1.加入对预选择条件的过滤
            string enterpriseKey = RequestHelper.GetValue("enterpriseKey");
            string salaryMonth = RequestHelper.GetValue("salaryMonth");
            if (GuidHelper.IsInvalidOrEmpty(enterpriseKey))
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "你没有选定企业信息，请选择。";
                infoList.Add(itemError);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }

            DateTime salaryDateFirstDay = DateTime.Today;
            if (string.IsNullOrWhiteSpace(salaryMonth))
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "你没有选定薪资月份，请选择。";
                infoList.Add(itemError);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                salaryMonth = HttpUtility.UrlDecode(salaryMonth);
                string salaryDateString = salaryMonth + "/1";
                salaryDateFirstDay = DateTimeHelper.Parse(salaryDateString, DateFormats.YMD);
            }

            //TODO:xieran20121101 因为要遍历企业中所有的人员，性能有可能不高，后续需要调整算法或者实现方式（异步？）
            List<LaborEntity> laborList = LaborBLL.Instance.GetLaborsByEnterprise(new Guid(enterpriseKey));
            List<LaborEntity> unMatchedLaborList = new List<LaborEntity>();
            List<SalarySummaryEntity> salarySummaryList = SalarySummaryBLL.Instance.GetList(enterpriseKey, salaryDateFirstDay);
            for (int j = laborList.Count - 1; j >= 0; j--)
            {
                LaborEntity laborItem = laborList[j];
                bool isMatch = false;
                for (int i = 0; i < salarySummaryList.Count; i++)
                {
                    if (laborItem.UserGuid.ToString() == salarySummaryList[i].LaborKey)
                    {
                        isMatch = true;
                        break;
                    }
                }

                if (isMatch == false)
                {
                    unMatchedLaborList.Add(laborItem);
                }
            }

            if (unMatchedLaborList.Count == 0)
            {
                SystemStatusInfo statusItem = new SystemStatusInfo();
                statusItem.SystemStatus = SystemStatuses.Success;
                statusItem.Message = "实付人员数量与应付人员数量相同！";
                infoList.Add(statusItem);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            else
            {
                SystemStatusInfo statusItem = new SystemStatusInfo();
                statusItem.SystemStatus = SystemStatuses.Warnning;
                statusItem.Message = "实付人员数量与应付人员数量有差别，具体如下人员应付而未付：";
                foreach (LaborEntity laborItem in unMatchedLaborList)
                {
                    statusItem.Message += string.Format("{0}({1})({2}), ", laborItem.UserNameCN, laborItem.LaborCode, laborItem.UserCardID);
                }
                infoList.Add(statusItem);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
        }
        #endregion

        #region 银行卡
        /// <summary>
        /// 劳务人员银行卡列表
        /// </summary>
        /// <returns></returns>
        /// <param name="itemKey">劳务人员标识</param>
        public ActionResult BankCardList(string itemKey)
        {
            List<BankEntity> entityList = new List<BankEntity>();

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                string whereClause = string.Format(" UserGuid='{0}' ", itemKey);
                string orderbyClause = string.Format("BankID DESC");
                entityList = BankBLL.Instance.GetList(whereClause, orderbyClause);
            }

            this.ViewBag.ItemKey = itemKey;
            return View(entityList);
        }


        public ActionResult BankCardItem(string userKey, string itemKey = StringHelper.Empty)
        {
            BankEntity entity = BankEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                entity = BankBLL.Instance.Get(itemKey);
            }

            entity.UserGuid = GuidHelper.TryConvert(userKey);
            return View(entity);
        }

        [HttpPost]
        public ActionResult BankCardItem(string itemKey, BankEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            BankEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetEntity = new BankEntity();
                targetEntity.UserGuid = RequestHelper.GetValue<Guid>("UserKey");

                SetBankCardEntityValue(originalEntity, ref  targetEntity);

                isSuccessful = BankBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = BankBLL.Instance.Get(itemKey);

                SetBankCardEntityValue(originalEntity, ref  targetEntity);
                isSuccessful = BankBLL.Instance.Update(targetEntity);
            }

            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功";
            }
            else
            {
                displayMessage = "数据保存失败";
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }

        private void SetBankCardEntityValue(BankEntity originalEntity, ref BankEntity targetEntity)
        {
            targetEntity.AccountName = originalEntity.AccountName;
            targetEntity.AccountNumber = originalEntity.AccountNumber;
            targetEntity.AccountStatus = originalEntity.AccountStatus;
            targetEntity.BankAddress = originalEntity.BankAddress;
            targetEntity.BankGuid = originalEntity.BankGuid;
            targetEntity.BankName = originalEntity.BankName;
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.IsPrimary = originalEntity.IsPrimary;
        }

        /// <summary>
        /// 批量开卡
        /// </summary>
        /// <returns></returns>
        public ActionResult BankCardBatch()
        {
            return View();
        }

        /// <summary>
        /// 批量开卡
        /// </summary>
        /// <param name="isOnlyPlaceHolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BankCardBatch(bool isOnlyPlaceHolder = true)
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
            string returnUrl = RequestHelper.CurrentFullUrl;

            string bankName = RequestHelper.GetValue("BankName");
            string bankBranch = RequestHelper.GetValue("Branch");

            int headerRowNumber = RequestHelper.GetValue<int>("headerRowNumber", 3);
            HttpPostedFile postedFile = RequestHelper.CurrentRequest.Files["fileSelector"];

            #region 条件判定（如果不满足基本条件直接跳出并提示）
            if (HttpPostedFileHelper.HasFile(postedFile) == false)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "没有选择Excel文件，请先选择文件然后再进行导入！";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
            #endregion

            try
            {
                #region 将数据读入内存
                int userCountSuccessful = 0;
                int userCountFailure = 0;
                string userListFailure = string.Empty;
                DataTable dataTable = ExcelHelper.ReadExcel(postedFile.InputStream, headerRowNumber);
                NameValueCollection bankCardItemMapData = (NameValueCollection)ConfigurationManager.GetSection("bankCardItemMap");

                List<string> columnNameList = new List<string>();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    columnNameList.Add(dataTable.Columns[i].ColumnName);
                }
                #endregion

                #region 对Excel的每行信息进行解析
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int dataRowNumberInExcel = headerRowNumber + i + 1;
                    try
                    {
                        DataRow row = dataTable.Rows[i];
                        if (row == null)
                        {
                            continue;
                        }

                        string laborUserNameCN = string.Empty;
                        string laborUserCardID = string.Empty;

                        Guid bankCardGuid = GuidHelper.NewGuid();
                        BankEntity bankEntity = new BankEntity();
                        bankEntity.BankGuid = bankCardGuid;
                        bankEntity.BankName = bankName;
                        bankEntity.Branch = bankBranch;

                        foreach (string columnName in columnNameList)
                        {
                            //1.根据Excel文件中的列名称映射银行卡实体的属性名称
                            string propertyName = bankCardItemMapData[columnName];
                            if (string.IsNullOrWhiteSpace(propertyName))
                            {
                                continue;
                            }

                            //2.给银行卡实体属性赋值
                            object cellValue = row[columnName];
                            if (cellValue != null)
                            {
                                switch (propertyName)
                                {
                                    case "UserNameCN":
                                        laborUserNameCN = cellValue.ToString();
                                        break;
                                    case "UserCardID":
                                        laborUserCardID = cellValue.ToString();
                                        break;
                                    case "AccountNumber":
                                        bankEntity.AccountNumber = cellValue.ToString();
                                        break;
                                    case "CompanyName":
                                        //TODO:xieran20121022暂时使用BankAddress记录公司信息
                                        bankEntity.BankAddress = cellValue.ToString();
                                        break;
                                    default:
                                        //do nothing.
                                        break;
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(laborUserNameCN) || string.IsNullOrWhiteSpace(laborUserCardID))
                        {
                            userCountFailure++;
                            userListFailure += string.Format("{0}({1}({2})请确认此用户的用户名称和身份证号均不可以为空), <br />", dataRowNumberInExcel, laborUserNameCN, laborUserCardID);
                        }
                        else
                        {
                            //根据人员姓名和身份证号，确认劳务人员的UserGuid
                            BusinessUser labor = BusinessUserBLL.GetByUserIDCard(laborUserCardID);
                            if (labor != null && labor.IsEmpty == false)
                            {
                                if (labor.UserNameCN == laborUserNameCN)
                                {
                                    bankEntity.AccountName = laborUserNameCN;
                                    bankEntity.UserGuid = labor.UserGuid;
                                    bankEntity.IsPrimary = Logics.True;

                                    bool isSuccessful = BankBLL.Instance.Create(bankEntity);
                                    if (isSuccessful == true)
                                    {
                                        userCountSuccessful++;
                                    }
                                    else
                                    {
                                        userCountFailure++;
                                        userListFailure += string.Format("{0}({1}({2})错误未知), <br />", dataRowNumberInExcel, laborUserNameCN, laborUserCardID);
                                    }
                                }
                                else
                                {
                                    userCountFailure++;
                                    userListFailure += string.Format("{0}({1}({2})请确认此用户的用户名称和身份证号是否跟系统内的数据一致), <br />", dataRowNumberInExcel, laborUserNameCN, laborUserCardID);
                                }
                            }
                            else
                            {
                                userCountFailure++;
                                userListFailure += string.Format("{0}({1}({2})请确认此用户的用户名称和身份证号是否跟系统内的数据一致), <br />", dataRowNumberInExcel, laborUserNameCN, laborUserCardID);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        userCountFailure++;
                        userListFailure += i + ", ";
                    }
                }
                #endregion

                #region 操作结果展示
                //A.1、操作结果（导入成功的人员信息）
                SystemStatusInfo itemSuccessful = new SystemStatusInfo();
                itemSuccessful.SystemStatus = SystemStatuses.Success;
                itemSuccessful.Message = string.Format("共有{0}人导入成功。", userCountSuccessful);
                infoList.Add(itemSuccessful);

                //A.2、操作结果（导入失败的人员信息）
                if (userCountFailure > 0)
                {
                    SystemStatusInfo itemError = new SystemStatusInfo();
                    itemError.SystemStatus = SystemStatuses.Failuer;
                    itemError.Message = string.Format("共有{0}人导入失败。", userCountFailure);
                    if (string.IsNullOrWhiteSpace(userListFailure) == false)
                    {
                        itemError.Message += string.Format("导入失败的人员分别位于{0}行, <br />", userListFailure);
                    }
                    infoList.Add(itemError);
                }
                #endregion
            }
            catch (OfficeXmlFileException)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = "请选择Excel2003格式的文件。你可以将本文件在Excel中另存的时候选择97/2003格式！";
                infoList.Add(itemError);
            }

            this.TempData.Add("OperationResultData", infoList);
            return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
        }
        #endregion

        #region 批量派工
        /// <summary>
        /// 批量派工
        /// </summary>
        /// <param name="itemKey">劳务人员标识</param>
        /// <returns></returns>
        public ActionResult BatchSettleWork(string itemKeys)
        {
            LaborContractEntity laborContractEntity = LaborContractEntity.Empty;
            this.ViewData["itemKeys"] = itemKeys;
            return View(laborContractEntity);
        }

        /// <summary>
        /// 批量派工
        /// </summary>
        /// <param name="itemKeys">劳务人员标识</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchSettleWork(string itemKeys, LaborContractEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            LaborContractEntity targetEntity = null;
            if (string.IsNullOrWhiteSpace(itemKeys) == true)
            {
                isSuccessful = false;
                displayMessage = "请先选择至少一个劳务人员，然后在为其派工，谢谢！";
            }
            else
            {
                targetEntity = new LaborContractEntity();
                SetTargetContractEntityValue(originalEntity, ref  targetEntity);
                targetEntity.OperateDate = DateTime.Now;
                targetEntity.OperateUserGuid = BusinessUserBLL.CurrentUser.UserGuid;

                try
                {
                    List<string> laborGuidList = JsonHelper.DeSerialize<List<string>>(itemKeys);
                    foreach (string item in laborGuidList)
                    {
                        Guid laborGuid = Converter.ChangeType<Guid>(item);
                        if (laborGuid != Guid.Empty)
                        {
                            targetEntity.LaborUserGuid = laborGuid;
                            isSuccessful = LaborContractBLL.Instance.Create(targetEntity);
                        }
                    }
                }
                catch
                { }
            }

            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功";
            }
            else
            {
                displayMessage = "数据保存失败";
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }
        #endregion
    }
}
