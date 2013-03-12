using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.BusinessCore.Enum;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.DataBase;
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
            bool isSelfData = RequestHelper.GetValue("isSelfData", false);
            string workStatus = RequestHelper.GetValue("workStatus", string.Empty);
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("Index", new { id = 1, isSelfData = isSelfData, workStatus = workStatus });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("LaborQuery", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPageForLaborList;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";
            if (isSelfData == true)
            {
                whereClause += string.Format(" AND ( ServiceUserGuid='{0}' OR BusinessUserGuid='{0}' OR SettleUserGuid='{0}' ) ", BusinessUserBLL.CurrentUserGuid);
            }

            if (string.IsNullOrWhiteSpace(workStatus) == false)
            {
                switch (workStatus.ToLower())
                {
                    case "new":
                        whereClause += string.Format(" AND LaborWorkStatus={0} ", (int)LaborWorkStatuses.NewWorker);
                        break;
                    case "on":
                        whereClause += string.Format(" AND LaborWorkStatus={0} ", (int)LaborWorkStatuses.Worked);
                        break;
                    case "off":
                        whereClause += string.Format(" AND ( LaborWorkStatus={0} OR LaborWorkStatus={1} OR LaborWorkStatus={2} )", (int)LaborWorkStatuses.NormalStop, (int)LaborWorkStatuses.UnNormalEnterpriseStop, (int)LaborWorkStatuses.UnNormalPersenalStop);
                        break;
                    default:
                        break;
                }

            }

            ////--数据权限----------------------------------------------------------------------
            //whereClause += " AND ( ";
            //whereClause += string.Format(" {0} ", PermissionDataHelper.GetFilterCondition("FinanceUserGuid"));
            //whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("ProviderUserGuid"));
            //whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("RecommendUserGuid"));
            //whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("ServiceUserGuid"));
            //whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("BusinessUserGuid"));
            //whereClause += string.Format(" OR {0} ", PermissionDataHelper.GetFilterCondition("SettleUserGuid"));
            //whereClause += " ) ";
            ////--end--------------------------------------------------------------------------

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
            dic["CurrentLaborDepartment"] = "所在部门";
            dic["CurrentLaborWorkShop"] = "所在车间";
            dic["LaborCode"] = "职工编号";
            dic["CurrentBankAccountNumber"] = "银行账户";
            dic["UserSex"] = "性别";
            dic["UserAge"] = "年龄";
            dic["UserBirthDay"] = "出生日期";
            dic["UserEducationalBackground"] = "学历";
            dic["UserEducationalSchool"] = "毕业学校";
            dic["SocialSecurityNumber"] = "社保号";
            dic["HouseHoldType"] = "户口性质";
            dic["UserMobileNO"] = "联系电话";
            dic["UrgentTelephone"] = "紧急联系电话";
            dic["CurrentContractStartDate"] = "最近合同开始时间";
            dic["CurrentContractStopDate"] = "最近合同到期时间";
            dic["CurrentContractDiscontinueDate"] = "最近离职时间";
            dic["InformationBrokerUserName"] = "信息员";
            dic["ProviderUserName"] = "信息提供人员";
            dic["RecommendUserName"] = "推荐人员";
            dic["FinanceUserName"] = "财务人员";
            dic["ServiceUserName"] = "客服人员";
            dic["BusinessUserName"] = "业务人员";
            dic["SettleUserName"] = "安置人员";
            dic["Memo1"] = "备注1";
            dic["Memo2"] = "备注2";
            dic["Memo3"] = "备注3";
            dic["Memo4"] = "备注4";
            dic["Memo5"] = "备注5";
            //dic["CurrentBank.AccountNumber"] = "银行账号测试2";

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
            else
            {
                bool isUsingCompress = RequestHelper.GetValue<bool>("isUsingCompress");
                if (isUsingCompress == true)
                {
                    returnUrl = CompressHelper.Decompress(returnUrl);
                }
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
                targetEntity.ComeFromType = ComeFromTypes.ManageWrite;
                //首次录入系统，劳务人员的状态为未激活
                targetEntity.UserStatus = UserStatuses.Unactivated;
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

            if (isSuccessful == true)
            {
                return Redirect(returnUrl);
            }
            else
            {
                List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
                SystemStatusInfo itemStatus = new SystemStatusInfo();
                itemStatus.SystemStatus = SystemStatuses.Failuer;
                itemStatus.Message = string.Format("{0}", displayMessage);
                infoList.Add(itemStatus);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
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
            targetEntity.UserCardID = originalEntity.UserCardID;
            targetEntity.UserHeight = originalEntity.UserHeight;
            targetEntity.UserWeight = originalEntity.UserWeight;
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

            //targetEntity.CurrentDispatchType = originalEntity.CurrentDispatchType;
            targetEntity.ComeFromType = originalEntity.ComeFromType;

            targetEntity.SocialSecurityNumber = originalEntity.SocialSecurityNumber;
            targetEntity.HouseHoldType = originalEntity.HouseHoldType;
            targetEntity.UserEducationalSchool = originalEntity.UserEducationalSchool;

            IDCard idCard = IDCard.Parse(targetEntity.UserCardID);

            targetEntity.UserSex = originalEntity.UserSex;
            if (targetEntity.UserSex == Sexes.UnSet)
            {
                targetEntity.UserSex = idCard.Sex;
            }

            targetEntity.UserBirthDay = originalEntity.UserBirthDay;
            if (targetEntity.UserBirthDay == DateTimeHelper.Min)
            {
                targetEntity.UserBirthDay = idCard.BirthDay;
            }

            targetEntity.IDCardPlace = originalEntity.IDCardPlace;
            if (string.IsNullOrWhiteSpace(targetEntity.IDCardPlace))
            {
                targetEntity.IDCardPlace = idCard.GetAddress();
            }

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
        /// 删除
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public ActionResult Delete(string itemKey)
        {
            LaborBLL.Instance.Delete(itemKey);
            string url = RequestHelper.GetValue("returnUrl");
            bool isUsingCompress = RequestHelper.GetValue<bool>("isUsingCompress");
            if (isUsingCompress == true)
            {
                url = CompressHelper.Decompress(url);
            }
            return Redirect(url);
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

                    //Hack:xieran20121109 为了匹配所有的服务角色的guid，这里获取全部内部员工的信息，如果以后员工人数数量
                    //增加到一个程度后，可能会有性能问题
                    List<BusinessUser> employeeList = BusinessUserBLL.GetList(UserTypes.Manager);

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

                            //从Excel导入的各个服务角色的姓名，以下将其转换为guid
                            laborEntity.ProviderUserGuid = GetEmployeeGuid(employeeList, laborEntity.ProviderUserName);
                            laborEntity.RecommendUserGuid = GetEmployeeGuid(employeeList, laborEntity.RecommendUserName);
                            laborEntity.FinanceUserGuid = GetEmployeeGuid(employeeList, laborEntity.FinanceUserName);
                            laborEntity.ServiceUserGuid = GetEmployeeGuid(employeeList, laborEntity.ServiceUserName);
                            laborEntity.BusinessUserGuid = GetEmployeeGuid(employeeList, laborEntity.BusinessUserName);
                            laborEntity.SettleUserGuid = GetEmployeeGuid(employeeList, laborEntity.SettleUserName);

                            laborEntity.InformationBrokerUserGuid = GetInformationBrokerGuid(laborEntity.InformationBrokerUserName);

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
                                userListFailure += string.Format("{0}(人员姓名和身份证同时为空), <br />", originalExcelRowNumber);
                            }
                            else
                            {
                                laborEntity.ComeFromType = ComeFromTypes.ManageBatch;
                                //首次录入系统，劳务人员的状态为未激活
                                laborEntity.UserStatus = UserStatuses.Unactivated;
                                CreateUserRoleStatuses createStatus = LaborBLL.Instance.Create(laborEntity);

                                if (createStatus == CreateUserRoleStatuses.Successful)
                                {
                                    userCountSuccessful++;
                                }
                                else
                                {
                                    userCountFailure++;
                                    userListFailure += string.Format("{0}({1}), <br />", originalExcelRowNumber, EnumHelper.GetDisplayValue(createStatus));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            userCountFailure++;
                            userListFailure += originalExcelRowNumber + ", <br />";
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
        /// 获取内部员工的Guid
        /// </summary>
        /// <param name="employeeList"></param>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        private Guid GetEmployeeGuid(List<BusinessUser> employeeList, string employeeName)
        {
            if (string.IsNullOrWhiteSpace(employeeName))
            {
                return Guid.Empty;
            }

            foreach (BusinessUser currentUser in employeeList)
            {
                if (currentUser.UserNameCN == employeeName)
                {
                    return currentUser.UserGuid;
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// 根据姓名匹配信息员的Guid
        /// </summary>
        /// <param name="informationBrokerUserName"></param>
        /// <returns></returns>
        private Guid GetInformationBrokerGuid(string informationBrokerUserName)
        {
            string whereClause = string.Format(" InformationBrokerName='{0}' OR InformationBrokerNameShort='{0}' ", informationBrokerUserName);
            List<InformationBrokerEntity> entityList = InformationBrokerBLL.Instance.GetList(whereClause);
            if (entityList == null || entityList.Count == 0 || entityList.Count > 1)
            {
                return Guid.Empty;
            }
            else
            {
                return entityList[0].InformationBrokerGuid;
            }
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

        #region 个人合同
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
                targetEntity.LaborUserGuid = RequestHelper.GetValue<Guid>(PassingParamValueSourceTypes.Form, "UserKey");
                targetEntity.OperateDate = DateTime.Now;
                targetEntity.OperateUserGuid = BusinessUserBLL.CurrentUser.UserGuid;

                SetTargetContractEntityValue(originalEntity, ref  targetEntity);

                if (targetEntity.LaborContractStopDate < targetEntity.LaborContractStartDate)
                {
                    return Json(new LogicStatusInfo(false, "合同结束时间不能早于合同开始时间，谢谢！"));
                }

                if (targetEntity.EnterpriseGuid == Guid.Empty)
                {
                    isSuccessful = false;
                    displayMessage = "请在智能提示的企业列表中选择企业信息后保存。";
                }
                else
                {
                    isSuccessful = LaborContractBLL.Instance.Create(targetEntity);
                }
            }
            else
            {
                targetEntity = LaborContractBLL.Instance.Get(itemKey);

                SetTargetContractEntityValue(originalEntity, ref  targetEntity);

                if (targetEntity.LaborContractStopDate < targetEntity.LaborContractStartDate)
                {
                    return Json(new LogicStatusInfo(false, "合同结束时间不能早于合同开始时间，谢谢！"));
                }

                if (targetEntity.LaborContractDiscontinueDate != DateTimeHelper.Min && targetEntity.LaborContractDiscontinueDate < targetEntity.LaborContractStartDate)
                {
                    return Json(new LogicStatusInfo(false, "合同终止时间不能早于合同开始时间，谢谢！"));
                }

                if (targetEntity.EnterpriseGuid == Guid.Empty)
                {
                    isSuccessful = false;
                    displayMessage = "请在智能提示的企业列表中选择企业信息后保存。";
                }
                else
                {
                    isSuccessful = LaborContractBLL.Instance.Update(targetEntity);
                }
            }

            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功。" + displayMessage;
            }
            else
            {
                displayMessage = "数据保存失败。" + displayMessage;
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }

        private void SetTargetContractEntityValue(LaborContractEntity originalEntity, ref LaborContractEntity targetEntity)
        {
            targetEntity.EnterpriseGuid = ControlHelper.GetRealValue<Guid>("EnterpriseName");
            targetEntity.LaborContractDetails = originalEntity.LaborContractDetails;
            targetEntity.EnterpriseContractGuid = originalEntity.EnterpriseContractGuid;
            targetEntity.LaborContractStartDate = originalEntity.LaborContractStartDate;

            if (originalEntity.LaborContractStopDate == DateTimeHelper.Min)
            {
                if (originalEntity.LaborContractStartDate != DateTimeHelper.Min)
                {
                    targetEntity.LaborContractStopDate = originalEntity.LaborContractStartDate.AddYears(2).AddDays(-1);
                }
            }
            else
            {
                targetEntity.LaborContractStopDate = originalEntity.LaborContractStopDate;
            }

            targetEntity.LaborContractStatus = originalEntity.LaborContractStatus;

            targetEntity.LaborCode = originalEntity.LaborCode;
            targetEntity.LaborContractIsCurrent = originalEntity.LaborContractIsCurrent;

            SetTargetContractCostValue(originalEntity, ref targetEntity);

            targetEntity.LaborContractDiscontinueDate = originalEntity.LaborContractDiscontinueDate;
            targetEntity.LaborContractDiscontinueDesc = originalEntity.LaborContractDiscontinueDesc;

            targetEntity.LaborWorkShop = originalEntity.LaborWorkShop;
            targetEntity.LaborDepartment = originalEntity.LaborDepartment;
            targetEntity.DispatchType = originalEntity.DispatchType;
        }

        /// <summary>
        /// 设置劳务人员合同的费用模式
        /// </summary>
        /// <param name="originalEntity"></param>
        /// <param name="targetEntity"></param>
        private static void SetTargetContractCostValue(LaborContractEntity originalEntity, ref LaborContractEntity targetEntity)
        {
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

            targetEntity.EnterpriseOtherInsuranceFormularKey = originalEntity.EnterpriseOtherInsuranceFormularKey;
            targetEntity.EnterpriseTaxFeeFormularKey = originalEntity.EnterpriseTaxFeeFormularKey;
        }
        #endregion

        #region 劳务人员合同查询
        public ActionResult ContractQueryList(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("ContractQueryList", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPageForLaborList;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            //处理在职时间段问题
            DateTime jobingDateStart = DateTimeHelper.Min;
            DateTime jobingDateEnd = DateTimeHelper.Min;
            if (whereClause.Contains("JobingDate"))
            {
                string jobingDateStringAll = whereClause.Substring(whereClause.IndexOf("JobingDate"));
                string jobingDateStringNoLeftBlacket = jobingDateStringAll.Substring(jobingDateStringAll.IndexOf("'") + 1);
                string jobingDateString = jobingDateStringNoLeftBlacket.Substring(0, jobingDateStringNoLeftBlacket.IndexOf("'"));
                jobingDateStart = Converter.ChangeType<DateTime>(jobingDateString, DateTimeHelper.Min);
            }

            if (whereClause.Contains("JobingDate"))
            {
                string jobingDateStringAll = whereClause.Substring(whereClause.LastIndexOf("JobingDate"));
                string jobingDateStringNoLeftBlacket = jobingDateStringAll.Substring(jobingDateStringAll.IndexOf("'") + 1);
                string jobingDateString = jobingDateStringNoLeftBlacket.Substring(0, jobingDateStringNoLeftBlacket.IndexOf("'"));
                jobingDateEnd = Converter.ChangeType<DateTime>(jobingDateString, DateTimeHelper.Min);
            }

            if ((jobingDateStart != DateTimeHelper.Min && jobingDateEnd == DateTimeHelper.Min) || (jobingDateStart == DateTimeHelper.Min && jobingDateEnd != DateTimeHelper.Min))
            {
                List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
                string returnUrl = Url.Action("ContractQueryList", new { id = 1 });
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Warnning;
                itemError.Message = "请同时选择在职时间开始和在职时间结束！";
                infoList.Add(itemError);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }

            if (jobingDateStart != DateTimeHelper.Min && jobingDateEnd != DateTimeHelper.Min)
            {
                whereClause = whereClause.Substring(0, whereClause.IndexOf("JobingDate"));
                whereClause = whereClause.TrimEnd(' ', '(').ToLower();

                if (whereClause.EndsWith("and") || whereClause.EndsWith("or"))
                {
                    string whereClauseForJobing = string.Format(@" (
                    	            ( LaborContractStartDate>='{1}' AND (LaborContractStartDate<='{2}') ) OR 
                    	            ( LaborContractDiscontinueDate>='{1}' AND (LaborContractDiscontinueDate<='{2}') ) OR
                    	            ( LaborContractStartDate<'{1}' AND (LaborContractDiscontinueDate>'{2}' OR LaborContractDiscontinueDate is null OR LaborContractDiscontinueDate='{0}' ))
                    	            ) ", DateTimeHelper.Min, jobingDateStart, jobingDateEnd);
                    whereClause += whereClauseForJobing;
                }
            }

            string orderClause = "LaborContractID DESC";

            PagedEntityCollection<LaborContractEntity> entityList = LaborContractBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<LaborContractEntity> pagedExList = new PagedList<LaborContractEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            bool isExportExcel = RequestHelper.GetValue("exportExcel", false);
            if (isExportExcel == true)
            {
                return LaborContractListToExcelFile(entityList.Records);
            }
            else
            {
                return View(pagedExList);
            }
        }

        private ActionResult LaborContractListToExcelFile(IList<LaborContractEntity> laborList)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Labor.UserNameCN"] = "人员名称";
            dic["Labor.UserCardID"] = "身份证号码";
            dic["Enterprise.CompanyNameShort"] = "企业名称";
            dic["LaborDepartment"] = "所在部门";
            dic["LaborWorkShop"] = "所在车间";
            dic["LaborContractStartDate"] = "入职时间";
            dic["LaborContractStopDate"] = "合同到期时间";
            dic["LaborContractDiscontinueDate"] = "离职时间";
            dic["LaborContractDiscontinueDesc"] = "离职原因";

            dic["Labor.InformationBrokerUserName"] = "信息员";
            dic["Labor.ServiceUserName"] = "客服人员";
            dic["Labor.BusinessUserName"] = "业务人员";
            dic["Labor.SettleUserName"] = "安置人员";

            Stream excelStream = ExcelHelper.WriteExcel(laborList, dic);
            return File(excelStream, ContentTypes.GetContentType("xls"), string.Format("劳务人员合同信息-{0}.xls", DateTime.Today.ToShortDateString()));
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

            bool isExportExcel = RequestHelper.GetValue("exportExcel", false);
            if (isExportExcel == true)
            {
                return SalaryListToExcelFile(entityList.Records);
            }
            else
            {
                return View(pagedExList);
            }
        }

        private ActionResult SalaryListToExcelFile(IList<SalarySummaryEntity> laborList)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["LaborName"] = "人员名称";
            dic["LaborCode"] = "职工编号";
            dic["Labor.CurrentBankAccountNumber"] = "银行账户";
            dic["SalaryNeedPayBeforeCost"] = "应付工资";
            dic["PersonInsuranceReal"] = "个人保险";
            dic["PersonReserveFundReal"] = "个人公积金";
            dic["PersonOtherCostReal"] = "补扣保险（滞纳金）";
            dic["SalaryNeedPayBeforeTax"] = "应税工资";
            dic["PersonBorrow"] = "扣其他（借款等）";
            dic["SalaryNeedPayToLabor"] = "实付";

            Stream excelStream = ExcelHelper.WriteExcel(laborList, dic);
            return File(excelStream, ContentTypes.GetContentType("xls"), string.Format("劳务人员薪资信息-{0}.xls", DateTime.Now.ToShortDateString()));
        }

        /// <summary>
        /// 对劳务人员薪资计算数据的修正
        /// </summary>
        /// <param name="itemKey">工资summary的Guid</param>
        /// <returns></returns>
        public ActionResult SalaryCalculateFix(string itemKey)
        {
            SalarySummaryEntity salarySummaryEntity = SalarySummaryBLL.Instance.Get(itemKey);

            return View(salarySummaryEntity);
        }

        /// <summary>
        /// 对劳务人员薪资计算数据的修正
        /// </summary>
        /// <param name="itemKey">工资summary的Guid</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryCalculateFix(string itemKey, SalarySummaryEntity salarySummaryEntity)
        {
            SalarySummaryEntity originalEntity = SalarySummaryBLL.Instance.Get(itemKey);

            //对几个计算数据的修正值进行赋值
            originalEntity.EnterpriseInsuranceCalculatedFix = salarySummaryEntity.EnterpriseInsuranceCalculatedFix;
            originalEntity.EnterpriseManageFeeCalculatedFix = salarySummaryEntity.EnterpriseManageFeeCalculatedFix;
            originalEntity.EnterpriseMixCostCalculatedFix = salarySummaryEntity.EnterpriseMixCostCalculatedFix;
            originalEntity.EnterpriseOtherCostCalculatedFix = salarySummaryEntity.EnterpriseOtherCostCalculatedFix;
            originalEntity.EnterpriseReserveFundCalculatedFix = salarySummaryEntity.EnterpriseReserveFundCalculatedFix;
            originalEntity.PersonInsuranceCalculatedFix = salarySummaryEntity.PersonInsuranceCalculatedFix;
            originalEntity.PersonManageFeeCalculatedFix = salarySummaryEntity.PersonManageFeeCalculatedFix;
            originalEntity.PersonMixCostCalculatedFix = salarySummaryEntity.PersonMixCostCalculatedFix;
            originalEntity.PersonOtherCostCalculatedFix = salarySummaryEntity.PersonOtherCostCalculatedFix;
            originalEntity.PersonReserveFundCalculatedFix = salarySummaryEntity.PersonReserveFundCalculatedFix;
            originalEntity.EnterpriseOtherInsuranceCalculatedFix = salarySummaryEntity.EnterpriseOtherInsuranceCalculatedFix;
            originalEntity.EnterpriseTaxFeeCalculatedFix = salarySummaryEntity.EnterpriseTaxFeeCalculatedFix;

            string displayMessage = string.Empty;
            bool isSuccessful = SalarySummaryBLL.Instance.Update(originalEntity);

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

        /// <summary>
        /// 费用批量修正
        /// </summary>
        /// <param name="itemKeys"></param>
        /// <returns></returns>
        public ActionResult SalaryFixBatch(string itemKeys)
        {
            return View();
        }

        /// <summary>
        /// 费用批量修正
        /// </summary>
        /// <param name="itemKeys"></param>
        /// <param name="isOnlyPlaceHolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryFixBatch(string itemKeys, bool isOnlyPlaceHolder = true)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(itemKeys) == true)
            {
                isSuccessful = false;
                displayMessage = "请先选择至少一条数据，谢谢！";
            }
            else
            {
                List<string> salarySummaryGuidList = JsonHelper.DeSerialize<List<string>>(itemKeys);
                if (salarySummaryGuidList.Count == 1 && salarySummaryGuidList[0].ToLower() == "on")
                {
                    isSuccessful = false;
                    displayMessage = "请先选择至少一条数据，谢谢！";
                }
                else
                {
                    decimal? EnterpriseInsuranceCalculatedFix = RequestHelper.GetValue<decimal?>("EnterpriseInsuranceCalculatedFix", null);
                    decimal? EnterpriseManageFeeCalculatedFix = RequestHelper.GetValue<decimal?>("EnterpriseManageFeeCalculatedFix", null);
                    decimal? EnterpriseMixCostCalculatedFix = RequestHelper.GetValue<decimal?>("EnterpriseMixCostCalculatedFix", null);
                    decimal? EnterpriseOtherCostCalculatedFix = RequestHelper.GetValue<decimal?>("EnterpriseOtherCostCalculatedFix", null);
                    decimal? EnterpriseReserveFundCalculatedFix = RequestHelper.GetValue<decimal?>("EnterpriseReserveFundCalculatedFix", null);
                    decimal? PersonInsuranceCalculatedFix = RequestHelper.GetValue<decimal?>("PersonInsuranceCalculatedFix", null);
                    decimal? PersonManageFeeCalculatedFix = RequestHelper.GetValue<decimal?>("PersonManageFeeCalculatedFix", null);
                    decimal? PersonMixCostCalculatedFix = RequestHelper.GetValue<decimal?>("PersonMixCostCalculatedFix", null);
                    decimal? PersonOtherCostCalculatedFix = RequestHelper.GetValue<decimal?>("PersonOtherCostCalculatedFix", null);
                    decimal? PersonReserveFundCalculatedFix = RequestHelper.GetValue<decimal?>("PersonReserveFundCalculatedFix", null);
                    decimal? EnterpriseOtherInsuranceCalculatedFix = RequestHelper.GetValue<decimal?>("EnterpriseOtherInsuranceCalculatedFix", null);
                    decimal? EnterpriseTaxFeeCalculatedFix = RequestHelper.GetValue<decimal?>("EnterpriseTaxFeeCalculatedFix", null);

                    foreach (string item in salarySummaryGuidList)
                    {
                        SalarySummaryEntity originalEntity = SalarySummaryBLL.Instance.Get(item);
                        if (originalEntity.IsEmpty == false)
                        {
                            originalEntity.EnterpriseInsuranceCalculatedFix = EnterpriseInsuranceCalculatedFix;
                            originalEntity.EnterpriseManageFeeCalculatedFix = EnterpriseManageFeeCalculatedFix;
                            originalEntity.EnterpriseMixCostCalculatedFix = EnterpriseMixCostCalculatedFix;
                            originalEntity.EnterpriseOtherCostCalculatedFix = EnterpriseOtherCostCalculatedFix;
                            originalEntity.EnterpriseReserveFundCalculatedFix = EnterpriseReserveFundCalculatedFix;
                            originalEntity.PersonInsuranceCalculatedFix = PersonInsuranceCalculatedFix;
                            originalEntity.PersonManageFeeCalculatedFix = PersonManageFeeCalculatedFix;
                            originalEntity.PersonMixCostCalculatedFix = PersonMixCostCalculatedFix;
                            originalEntity.PersonOtherCostCalculatedFix = PersonOtherCostCalculatedFix;
                            originalEntity.PersonReserveFundCalculatedFix = PersonReserveFundCalculatedFix;
                            originalEntity.EnterpriseOtherInsuranceCalculatedFix = EnterpriseOtherInsuranceCalculatedFix;
                            originalEntity.EnterpriseTaxFeeCalculatedFix = EnterpriseTaxFeeCalculatedFix;

                            displayMessage = string.Empty;
                            SalarySummaryBLL.Instance.Update(originalEntity);
                        }
                    }

                    isSuccessful = true;
                }
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
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
            string salaryMonth = RequestHelper.GetValue(PassingParamValueSourceTypes.Form, "salaryMonth", "");
            DateTime salaryMonthDate = DateTimeHelper.Min;
            LaborEntity labor = LaborEntity.Empty;
            string salarySettlementStart = RequestHelper.GetValue(PassingParamValueSourceTypes.Form, "SettlementStartDate", "");
            string salarySettlementEnd = RequestHelper.GetValue(PassingParamValueSourceTypes.Form, "SettlementEndDate", "");
            DateTime salarySettlementStartDate = Converter.ChangeType(salarySettlementStart, DateTimeHelper.Min);
            DateTime salarySettlementEndDate = Converter.ChangeType(salarySettlementEnd, DateTimeHelper.Min); ;

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

            if (salarySettlementStartDate == DateTimeHelper.Min)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = "请选择结算开始日期信息";
                infoList.Add(itemError);

                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }

            if (salarySettlementEndDate == DateTimeHelper.Min)
            {
                SystemStatusInfo itemError = new SystemStatusInfo();
                itemError.SystemStatus = SystemStatuses.Failuer;
                itemError.Message = "请选择结算结束日期信息";
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
            SalarySummaryEntity salarySummaryEntity = SalarySummaryBLL.Instance.Get(enterpriseKey, labor.UserGuid.ToString(), salaryMonthDate);

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
                salarySummaryEntity.SalarySettlementStartDate = salarySettlementStartDate;
                salarySummaryEntity.SalarySettlementEndDate = salarySettlementEndDate;

                //Logics isFirstCash = SalarySummaryBLL.Instance.IsFirstCash(enterpriseKey, labor.UserGuid.ToString());
                //salarySummaryEntity.IsFirstCash = isFirstCash;

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
                        salarySummaryEntity.SalaryGrossPay += itemValueDelta;
                        salarySummaryEntity.SalaryCashDate = entity.SalaryItemCashDate;
                        break;
                    case SalaryItemKinds.Rewards:
                        salarySummaryEntity.SalaryGrossPay += itemValueDelta;
                        break;
                    case SalaryItemKinds.Rebate:
                        //确保扣费为负值
                        salarySummaryEntity.SalaryRebate += (0 - Math.Abs(itemValueDelta));
                        break;
                    case SalaryItemKinds.EnterpriseInsurance:
                        salarySummaryEntity.EnterpriseInsuranceReal += itemValueDelta;
                        salarySummaryEntity.InsuranceCashDate = entity.SalaryItemCashDate;
                        break;
                    case SalaryItemKinds.EnterpriseReserveFund:
                        salarySummaryEntity.EnterpriseReserveFundReal += itemValueDelta;
                        salarySummaryEntity.ReserveFundCashDate = entity.SalaryItemCashDate;
                        break;
                    case SalaryItemKinds.EnterpriseManageFee:
                        salarySummaryEntity.EnterpriseManageFeeReal += itemValueDelta;
                        salarySummaryEntity.EnterpriseManageFeeCashDate = entity.SalaryItemCashDate;
                        break;
                    case SalaryItemKinds.EnterpriseOtherFee:
                        salarySummaryEntity.EnterpriseOtherCostReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.PersonInsurance:
                        salarySummaryEntity.PersonInsuranceReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.PersonReserveFund:
                        salarySummaryEntity.PersonReserveFundReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.PersonalOtherFee:
                        salarySummaryEntity.PersonOtherCostReal += itemValueDelta;
                        break;
                    case SalaryItemKinds.EnterpriseGeneralRecruitFee:
                        salarySummaryEntity.EnterpriseGeneralRecruitFeeReal += itemValueDelta;
                        salarySummaryEntity.EnterpriseGeneralRecruitFeeCashDate = entity.SalaryItemCashDate;
                        break;
                    case SalaryItemKinds.EnterpriseOnceRecruitFee:
                        salarySummaryEntity.EnterpriseOnceRecruitFeeReal += itemValueDelta;
                        salarySummaryEntity.EnterpriseOnceRecruitFeeCashDate = entity.SalaryItemCashDate;
                        break;
                    case SalaryItemKinds.SalaryTax:
                        //do nothing.(工资税系统自动计算，不能录入)
                        break;
                    case SalaryItemKinds.EnterpriseOtherInsurance:
                        salarySummaryEntity.EnterpriseOtherInsuranceReal += itemValueDelta;
                        salarySummaryEntity.EnterpriseOtherInsuranceCashDate = entity.SalaryItemCashDate;
                        break;
                    case SalaryItemKinds.EnterpriseTaxFee:
                        salarySummaryEntity.EnterpriseTaxFeeReal += itemValueDelta;
                        salarySummaryEntity.EnterpriseTaxFeeCashDate = entity.SalaryItemCashDate;
                        break;
                    default:
                        break;
                }

                SalarySummaryBLL.Instance.Update(salarySummaryEntity);
            }

            return RedirectToAction("SalaryDetailsList", new { itemKey = entity.SalarySummaryKey });
        }


        public ActionResult SalaryDeleteByEnterprise()
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();
            string returnUrl = Url.Action("SalaryListPreSelector");

            string enterpriseKey = RequestHelper.GetValue("EnterpriseName_Value");
            string enterpriseName = RequestHelper.GetValue("EnterpriseName");
            string salaryMonth = RequestHelper.GetValue("SalaryMonth");
            salaryMonth = HttpUtility.UrlDecode(salaryMonth);
            string salaryDateString = salaryMonth + "/1";

            SalarySummaryBLL.Instance.DeleteList(enterpriseKey, Converter.ChangeType(salaryDateString, DateTimeHelper.Min));

            SystemStatusInfo itemError = new SystemStatusInfo();
            itemError.SystemStatus = SystemStatuses.Success;
            itemError.Message = string.Format("企业【{0}】【{1}】月份的薪资数据已经被成功删除。", enterpriseName, salaryMonth);
            infoList.Add(itemError);
            this.TempData.Add("OperationResultData", infoList);
            return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
        }

        /// <summary>
        /// 删除工资
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public ActionResult SalaryDelete(string itemKey)
        {
            //1.删除Summary内的信息
            bool isSuccessful = SalarySummaryBLL.Instance.Delete(itemKey);

            //2.删除Details内的信息
            if (isSuccessful == true)
            {
                Guid summaryGuid = GuidHelper.TryConvert(itemKey);
                if (summaryGuid != Guid.Empty)
                {
                    SalaryDetailsBLL.Instance.DeleteList(summaryGuid);
                }
            }

            string url = RequestHelper.GetValue("returnUrl");
            bool isUsingCompress = RequestHelper.GetValue<bool>("isUsingCompress");
            if (isUsingCompress == true)
            {
                url = CompressHelper.Decompress(url);
            }
            return Redirect(url);
        }

        /// <summary>
        /// 删除工资
        /// </summary>
        /// <param name="itemKeys"></param>
        /// <returns></returns>
        public ActionResult SalaryDeleteBatch(string itemKeys)
        {
            return View();
        }

        /// <summary>
        /// 删除工资
        /// </summary>
        /// <param name="itemKeys"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryDeleteBatch(string itemKeys, bool isOnlyPlaceHolder = true)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(itemKeys) == true)
            {
                isSuccessful = false;
                displayMessage = "请先选择至少一条数据，谢谢！";
            }
            else
            {
                List<string> salarySummaryGuidList = JsonHelper.DeSerialize<List<string>>(itemKeys);
                if (salarySummaryGuidList.Count == 1 && salarySummaryGuidList[0].ToLower() == "on")
                {
                    isSuccessful = false;
                    displayMessage = "请先选择至少一条数据，谢谢！";
                }
                else
                {
                    foreach (string item in salarySummaryGuidList)
                    {
                        //1.删除Summary内的信息
                        isSuccessful = SalarySummaryBLL.Instance.Delete(item);

                        //2.删除Details内的信息
                        if (isSuccessful == true)
                        {
                            Guid summaryGuid = GuidHelper.TryConvert(item);
                            if (summaryGuid != Guid.Empty)
                            {
                                SalaryDetailsBLL.Instance.DeleteList(summaryGuid);
                            }
                        }
                    }
                }
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));

            //string url = RequestHelper.GetValue("returnUrl");
            //bool isUsingCompress = RequestHelper.GetValue<bool>("isUsingCompress");
            //if (isUsingCompress == true)
            //{
            //    url = CompressHelper.Decompress(url);
            //}

            //url = RequestHelper.DecodeUrl(url);
            //return Redirect(url);
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

            string SettlementStartDateString = RequestHelper.GetValue("SettlementStartDate");
            DateTime SettlementStartDate = Converter.ChangeType(SettlementStartDateString, DateTime.Today);

            string SettlementEndDateString = RequestHelper.GetValue("SettlementEndDate");
            DateTime SettlementEndDate = Converter.ChangeType(SettlementEndDateString, DateTime.Today);

            string SalaryCashDateString = RequestHelper.GetValue("SalaryCashDate");
            DateTime SalaryCashDate = Converter.ChangeType(SalaryCashDateString, DateTime.Today);

            string ManageFeeCashDateString = RequestHelper.GetValue("ManageFeeCashDate");
            DateTime ManageFeeCashDate = Converter.ChangeType(ManageFeeCashDateString, DateTime.Today);

            string InsuranceCashDateString = RequestHelper.GetValue("InsuranceCashDate");
            DateTime InsuranceCashDate = Converter.ChangeType(InsuranceCashDateString, DateTime.Today);

            string ReserveFundDateString = RequestHelper.GetValue("ReserveFundDate");
            DateTime ReserveFundDate = Converter.ChangeType(ReserveFundDateString, DateTime.Today);

            string GeneralRecruitDateString = RequestHelper.GetValue("GeneralRecruitDate");
            DateTime GeneralRecruitDate = Converter.ChangeType(GeneralRecruitDateString, DateTime.Today);

            string OnceRecruitDateString = RequestHelper.GetValue("OnceRecruitDate");
            DateTime OnceRecruitDate = Converter.ChangeType(OnceRecruitDateString, DateTime.Today);

            string OtherInsuranceCashDateString = RequestHelper.GetValue("OtherInsuranceCashDate");
            DateTime OtherInsuranceCashDate = Converter.ChangeType(OtherInsuranceCashDateString, DateTime.Today);

            string TaxFeeCashDateString = RequestHelper.GetValue("TaxFeeCashDate");
            DateTime TaxFeeCashDate = Converter.ChangeType(TaxFeeCashDateString, DateTime.Today);
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
                        string LaborUserCardIDForSalarySummary = string.Empty;

                        Guid salarySummaryGuid = GuidHelper.NewGuid();
                        SalarySummaryEntity salarySummaryEntity = new SalarySummaryEntity();
                        salarySummaryEntity.SalarySummaryGuid = salarySummaryGuid;
                        salarySummaryEntity.SalaryDate = salaryDate;
                        salarySummaryEntity.CreateDate = DateTime.Today;
                        salarySummaryEntity.CreateUserKey = BusinessUserBLL.CurrentUserGuid.ToString();
                        salarySummaryEntity.EnterpriseKey = enterpriseGuid.ToString();
                        salarySummaryEntity.SalaryPayStatus = SalaryPayStatuses.PaidToOrgnization;
                        salarySummaryEntity.SalarySettlementStartDate = SettlementStartDate;
                        salarySummaryEntity.SalarySettlementEndDate = SettlementEndDate;

                        foreach (string columnName in columnNameList)
                        {
                            //0.处理特殊的标题头信息
                            if (columnName.Contains("[忽略]"))
                            {
                                continue;
                            }

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

                                decimal salaryItemValue = Converter.ChangeType<decimal>(cellValue);
                                string columnNameEdited = columnName;
                                string nagetiveString = "[负值]";
                                string rebateBeforTaxString = "[先扣]";
                                string rebaseString = "[后扣]";
                                bool isRebateBeforeTax = false;

                                if (columnName.Contains(nagetiveString))
                                {
                                    salaryItemValue = 0 - Math.Abs(salaryItemValue);
                                    columnNameEdited = columnName.Remove(columnName.IndexOf(nagetiveString), nagetiveString.Length);
                                    if (columnName.Contains(rebateBeforTaxString))
                                    {
                                        columnNameEdited = columnNameEdited.Remove(columnNameEdited.IndexOf(rebateBeforTaxString), rebateBeforTaxString.Length);
                                        isRebateBeforeTax = true;
                                    }

                                    if (columnName.Contains(rebaseString))
                                    {
                                        columnNameEdited = columnNameEdited.Remove(columnNameEdited.IndexOf(rebaseString), rebaseString.Length);
                                    }
                                }

                                salaryDetailsEntity.SalaryItemKey = columnNameEdited;

                                switch (propertyName)
                                {
                                    case "UserNameCN":
                                        LaborUserNameCNForSalarySummary = cellValue.ToString();
                                        break;
                                    case "LaborCode":
                                        LaborUserCodeForSalarySummary = cellValue.ToString();
                                        break;
                                    case "UserCardID":
                                        LaborUserCardIDForSalarySummary = cellValue.ToString();
                                        break;
                                    case "EnterpriseMixCost":
                                        salarySummaryEntity.EnterpriseMixCostReal = salaryItemValue;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.EnterpriseMixCost);
                                        break;
                                    case "EnterpriseInsurance":
                                        salarySummaryEntity.EnterpriseInsuranceReal = salaryItemValue;
                                        salarySummaryEntity.InsuranceCashDate = InsuranceCashDate;
                                        salaryDetailsEntity.SalaryItemCashDate = InsuranceCashDate;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.EnterpriseInsurance);
                                        break;
                                    case "EnterpriseReserveFund":
                                        salarySummaryEntity.EnterpriseReserveFundReal = salaryItemValue;
                                        salarySummaryEntity.ReserveFundCashDate = ReserveFundDate;
                                        salaryDetailsEntity.SalaryItemCashDate = ReserveFundDate;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.EnterpriseReserveFund);
                                        break;
                                    case "EnterpriseManageFee":
                                        salarySummaryEntity.EnterpriseManageFeeReal = salaryItemValue;
                                        salarySummaryEntity.EnterpriseManageFeeCashDate = ManageFeeCashDate;
                                        salaryDetailsEntity.SalaryItemCashDate = ManageFeeCashDate;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.EnterpriseManageFee);
                                        break;
                                    case "EnterpriseGeneralRecruitFee":
                                        salarySummaryEntity.EnterpriseGeneralRecruitFeeReal = salaryItemValue;
                                        salarySummaryEntity.EnterpriseGeneralRecruitFeeCashDate = GeneralRecruitDate;
                                        salaryDetailsEntity.SalaryItemCashDate = GeneralRecruitDate;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.EnterpriseGeneralRecruitFee);
                                        break;
                                    case "EnterpriseOnceRecruitFee":
                                        salarySummaryEntity.EnterpriseOnceRecruitFeeReal = salaryItemValue;
                                        salarySummaryEntity.EnterpriseOnceRecruitFeeCashDate = OnceRecruitDate;
                                        salaryDetailsEntity.SalaryItemCashDate = OnceRecruitDate;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.EnterpriseOnceRecruitFee);
                                        break;
                                    case "PersonMixCost":
                                        salarySummaryEntity.PersonMixCostReal = salaryItemValue;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.PersonMixCost);
                                        break;
                                    case "PersonInsurance":
                                        salarySummaryEntity.PersonInsuranceReal = salaryItemValue;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.PersonInsurance);
                                        break;
                                    case "PersonReserveFund":
                                        salarySummaryEntity.PersonReserveFundReal = salaryItemValue;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.PersonReserveFund);
                                        break;
                                    case "PersonInsuranceAdditional":
                                        salarySummaryEntity.PersonOtherCostReal += salaryItemValue;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.PersonalOtherFee);
                                        break;
                                    case "PersonInsuranceOverdueFee":
                                        salarySummaryEntity.PersonOtherCostReal += salaryItemValue;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.PersonalOtherFee);
                                        break;
                                    case "PersonBorrow":
                                        salarySummaryEntity.PersonBorrow = salaryItemValue;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.PersonBorrow);
                                        break;
                                    case "EnterpriseOtherInsurance":
                                        salarySummaryEntity.EnterpriseOtherInsuranceReal += salaryItemValue;
                                        salarySummaryEntity.EnterpriseOtherInsuranceCashDate = OtherInsuranceCashDate;
                                        salaryDetailsEntity.SalaryItemCashDate = OtherInsuranceCashDate;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.EnterpriseOtherInsurance);
                                        break;
                                    case "EnterpriseTaxFee":
                                        salarySummaryEntity.EnterpriseTaxFeeReal += salaryItemValue;
                                        salarySummaryEntity.EnterpriseTaxFeeCashDate = TaxFeeCashDate;
                                        salaryDetailsEntity.SalaryItemCashDate = TaxFeeCashDate;
                                        SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.EnterpriseTaxFee);
                                        break;
                                    default:
                                        if (salaryItemValue >= 0)
                                        {
                                            salarySummaryEntity.SalaryGrossPay += salaryItemValue;
                                            SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.Rewards);
                                        }
                                        else
                                        {
                                            if (isRebateBeforeTax == true)
                                            {
                                                salarySummaryEntity.SalaryRebateBeforeTax += salaryItemValue;
                                            }
                                            else
                                            {
                                                salarySummaryEntity.SalaryRebate += salaryItemValue;
                                            }

                                            SetAndSaveSalaryDetailsItem(salaryItemValue, salaryDetailsEntity, SalaryItemKinds.Rebate);
                                        }
                                        break;
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(LaborUserNameCNForSalarySummary))
                        {
                            userCountFailure++;
                            userListFailure += string.Format("{0}({1}({2})({3})请确认此用户的用户名称不可以为空), <br />", dataRowNumberInExcel, LaborUserNameCNForSalarySummary, LaborUserCodeForSalarySummary, LaborUserCardIDForSalarySummary);
                            //物理删除掉已经插入的无效的salaryDetails数据
                            SalaryDetailsBLL.Instance.DeleteList(salarySummaryEntity.SalarySummaryGuid);
                        }
                        else
                        {
                            //根据人员姓名和工号，确认劳务人员的UserGuid
                            bool isMatchedLabor = false;
                            LaborEntity laborEntity = LaborBLL.Instance.Get(LaborUserNameCNForSalarySummary, LaborUserCodeForSalarySummary, LaborUserCardIDForSalarySummary, enterpriseGuid.ToString());
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
                                SalarySummaryEntity salarySummaryEntityConfirm = SalarySummaryBLL.Instance.Get(enterpriseGuid.ToString(), laborEntity.UserGuid.ToString(), salaryDate);
                                if (salarySummaryEntityConfirm.IsEmpty)
                                {
                                    salarySummaryEntity.LaborKey = laborEntity.UserGuid.ToString();
                                    salarySummaryEntity.LaborCode = LaborUserCodeForSalarySummary;
                                    salarySummaryEntity.LaborName = LaborUserNameCNForSalarySummary;

                                    //Logics isFirstCash = SalarySummaryBLL.Instance.IsFirstCash(enterpriseGuid.ToString(), laborEntity.UserGuid.ToString());
                                    //salarySummaryEntity.IsFirstCash = isFirstCash;
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
                                userListFailure += string.Format("{0}({1}({2})({3})请确认此用户的用户名称、工号和身份证号是否跟系统内的数据一致), <br />", dataRowNumberInExcel, LaborUserNameCNForSalarySummary, LaborUserCodeForSalarySummary, LaborUserCardIDForSalarySummary);
                                //物联删除掉已经插入的无效的salaryDetails数据
                                SalaryDetailsBLL.Instance.DeleteList(salarySummaryEntity.SalarySummaryGuid);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //try
                        //{
                        //    BusinessLogEntity log = new BusinessLogEntity();
                        //    log.LogCategory = "BatchSalaryImport";
                        //    log.LogMessage = ExceptionHelper.GetExceptionMessage(ex);
                        //    log.LogDate = DateTime.Now;

                        //    BusinessLogBLL.Instance.Create(log);
                        //}
                        //catch 
                        //{ 
                        //    //do nothing;
                        //}

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
            return View();
        }

        /// <summary>
        /// 薪资人数的应付数与实付数校验
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SalaryListCheck(bool isOnlyPlaceHolder = true)
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
            DateTime salaryDateStart = DateTime.Today;
            DateTime salaryDateEnd = DateTime.Today;

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

            salaryDateStart = RequestHelper.GetValue<DateTime>("JobingDateStart", DateTimeHelper.Min);
            salaryDateEnd = RequestHelper.GetValue<DateTime>("JobingDateEnd", DateTimeHelper.Min);
            if (salaryDateStart == DateTimeHelper.Min && salaryDateEnd == DateTimeHelper.Min)
            {
                salaryDateStart = DateTimeHelper.GetFirstDateOfMonth(salaryDateFirstDay);
                salaryDateEnd = DateTimeHelper.GetLastDateOfMonth(salaryDateFirstDay);
            }

            List<LaborContractEntity> laborContractList = LaborContractBLL.Instance.GetList(salaryDateStart, salaryDateEnd, new Guid(enterpriseKey));
            List<LaborEntity> unMatchedLaborList = new List<LaborEntity>();
            List<SalarySummaryEntity> salarySummaryList = SalarySummaryBLL.Instance.GetList(enterpriseKey, salaryDateFirstDay);
            for (int j = laborContractList.Count - 1; j >= 0; j--)
            {
                LaborContractEntity laborItem = laborContractList[j];
                bool isMatch = false;
                for (int i = 0; i < salarySummaryList.Count; i++)
                {
                    if (laborItem.LaborUserGuid.ToString() == salarySummaryList[i].LaborKey)
                    {
                        isMatch = true;
                        break;
                    }
                }

                if (isMatch == false)
                {
                    unMatchedLaborList.Add(laborItem.Labor);
                }
            }

            if (unMatchedLaborList.Count == 0)
            {
                SystemStatusInfo statusItem = new SystemStatusInfo();
                statusItem.SystemStatus = SystemStatuses.Success;
                statusItem.Message = "没有应付而未付的人员！";
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
                    statusItem.Message += string.Format("{0}({1})({2}), <br />", laborItem.UserNameCN, laborItem.LaborCode, laborItem.UserCardID);
                }
                infoList.Add(statusItem);
                this.TempData.Add("OperationResultData", infoList);
                return RedirectToAction("OperationResults", "System", new { returnUrl = returnUrl });
            }
        }

        /// <summary>
        /// 劳务人员结算查询
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryBalanceCheck(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("SalaryBalanceCheck", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            //SalaryDate
            DateTime salaryDate = DateTimeHelper.Min;
            if (whereClause.Contains("SalaryDate"))
            {
                string salaryDateStringAll = whereClause.Substring(whereClause.IndexOf("SalaryDate"));
                string salaryDateStringNoLeftBlacket = salaryDateStringAll.Substring(salaryDateStringAll.IndexOf("'") + 1);
                string salaryDateString = salaryDateStringNoLeftBlacket.Substring(0, salaryDateStringNoLeftBlacket.IndexOf("'"));
                salaryDate = Converter.ChangeType<DateTime>(salaryDateString, DateTimeHelper.Min);
            }

            if (salaryDate != DateTimeHelper.Min)
            {
                whereClause = whereClause.Substring(0, whereClause.IndexOf("SalaryDate"));
                whereClause = whereClause.TrimEnd(' ', '(').ToLower();

                if (whereClause.EndsWith("and") || whereClause.EndsWith("or"))
                {
                    whereClause += string.Format(" SalaryDate='{0}' ", salaryDate.ToShortDateString());
                }
            }

            string orderClause = "SalarySummaryID DESC";

            PagedEntityCollection<SalarySummaryEntity> entityList = SalarySummaryBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<SalarySummaryEntity> pagedExList = new PagedList<SalarySummaryEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            bool isExportExcel = RequestHelper.GetValue("exportExcel", false);
            if (isExportExcel == true)
            {
                return SalaryBalanceToExcelFile(entityList.Records);
            }
            else
            {
                return View(pagedExList);
            }
        }

        private ActionResult SalaryBalanceToExcelFile(IList<SalarySummaryEntity> salarySummaryList)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Labor.UserNameCN"] = "人员名称";
            dic["Labor.UserCardID"] = "身份证号码";
            dic["Enterprise.CompanyName"] = "务工企业";
            dic["SalaryDate"] = "结算月份";
            dic["SalarySettlementStartDate"] = "结算开始时间";
            dic["SalarySettlementEndDate"] = "结算结束时间";
            dic["Labor.CurrentContractStartDate"] = "入职时间";
            dic["Labor.CurrentContractDiscontinueDate"] = "离职时间";
            dic["EnterpriseManageFeeReal"] = "管理费";
            dic["EnterpriseManageFeeCashDate"] = "管理费回款时间";
            dic["EnterpriseGeneralRecruitFeeReal"] = "分次性招工费";
            dic["EnterpriseGeneralRecruitFeeCashDate"] = "分次性招工费回款时间";
            dic["EnterpriseOnceRecruitFeeReal"] = "一次性招工费";
            dic["EnterpriseOnceRecruitFeeCashDate"] = "一次性回款时间";
            //dic["UserEducationalBackground"] = "企业信息提供人";
            dic["Enterprise.ManageUserName"] = "企业开发人员";
            dic["Labor.InformationBrokerUserName"] = "劳务人员信息员";
            dic["Labor.ServiceUserName"] = "劳务人员客服人员";
            dic["Labor.SettleUserName"] = "劳务人员安置人员";
            dic["Labor.BusinessUserName"] = "劳务人员业务人员";

            Stream excelStream = ExcelHelper.WriteExcel(salarySummaryList, dic);
            return File(excelStream, ContentTypes.GetContentType("xls"), string.Format("劳务人员结算信息({0}).xls", GuidHelper.NewGuidString()));
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
            targetEntity.UserGuid = originalEntity.UserGuid;
            targetEntity.AccountName = originalEntity.AccountName;
            targetEntity.AccountNumber = originalEntity.AccountNumber;
            targetEntity.AccountStatus = originalEntity.AccountStatus;
            targetEntity.BankAddress = originalEntity.BankAddress;
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
                                        //Hack:xieran20121022 暂时使用BankAddress记录公司信息
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

        #region 批量派工，批量设置费用模式
        /// <summary>
        /// 批量设置费用模式
        /// </summary>
        /// <param name="itemKeys">劳务人员标识</param>
        /// <returns></returns>
        public ActionResult BatchSettleCost(string itemKeys)
        {
            LaborContractEntity laborContractEntity = LaborContractEntity.Empty;
            this.ViewData["itemKeys"] = itemKeys;
            return View(laborContractEntity);
        }

        /// <summary>
        /// 批量设置费用模式
        /// </summary>
        /// <param name="itemKeys">劳务人员标识</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchSettleCost(string itemKeys, LaborContractEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            LaborContractEntity targetEntity = null;
            if (string.IsNullOrWhiteSpace(itemKeys) == true)
            {
                isSuccessful = false;
                displayMessage = "请先选择至少一个劳务人员，然后在为其设置费用模式，谢谢！";
            }
            else
            {
                try
                {
                    List<string> laborGuidList = JsonHelper.DeSerialize<List<string>>(itemKeys);
                    if (laborGuidList.Count == 1 && laborGuidList[0].ToLower() == "on")
                    {
                        isSuccessful = false;
                        displayMessage = "请先选择至少一个劳务人员，然后在为其设置费用模式，谢谢！";
                    }
                    else
                    {
                        foreach (string item in laborGuidList)
                        {
                            Guid laborGuid = Converter.ChangeType<Guid>(item);
                            if (laborGuid != Guid.Empty)
                            {
                                targetEntity = LaborContractBLL.Instance.GetCurrentContract(laborGuid);
                                if (targetEntity.IsEmpty == false)
                                {
                                    SetTargetContractCostValue(originalEntity, ref  targetEntity);
                                    isSuccessful = LaborContractBLL.Instance.Update(targetEntity);
                                }
                            }
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
                displayMessage = "数据保存失败。" + displayMessage;
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }


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
                if (targetEntity.EnterpriseGuid == Guid.Empty)
                {
                    isSuccessful = false;
                    displayMessage = "请在智能提示的企业列表中选择企业信息后保存。";
                }
                else
                {
                    targetEntity.OperateDate = DateTime.Now;
                    targetEntity.OperateUserGuid = BusinessUserBLL.CurrentUser.UserGuid;

                    try
                    {
                        List<string> laborGuidList = JsonHelper.DeSerialize<List<string>>(itemKeys);
                        if (laborGuidList.Count == 1 && laborGuidList[0].ToLower() == "on")
                        {
                            isSuccessful = false;
                            displayMessage = "请先选择至少一个劳务人员，然后在为其派工，谢谢！";
                        }
                        else
                        {
                            foreach (string item in laborGuidList)
                            {
                                Guid laborGuid = Converter.ChangeType<Guid>(item);
                                if (laborGuid != Guid.Empty)
                                {
                                    targetEntity.LaborContractGuid = GuidHelper.NewGuid();
                                    targetEntity.LaborUserGuid = laborGuid;

                                    isSuccessful = LaborContractBLL.Instance.Create(targetEntity);
                                }
                            }
                        }
                    }
                    catch
                    { }
                }
            }

            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功";
            }
            else
            {
                displayMessage = "数据保存失败。" + displayMessage;
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }
        #endregion

        #region 统计汇总
        /// <summary>
        /// 统计汇总选择器
        /// </summary>
        /// <returns></returns>
        public ActionResult StatisticalSummarySelector()
        {
            return View();
        }

        public ActionResult StatisticalSummaryList()
        {
            string resourceName = RequestHelper.GetValue("ResourceName");
            string resourceValue = RequestHelper.GetValue("ResourceName_Value");
            string resourceType = RequestHelper.GetValue("ResourceName_AddonData");

            string enterpriseKey = ControlHelper.GetRealValue("EnterpriseName", string.Empty);

            string dispatchTypeString = RequestHelper.GetValue("DispatchType");
            DispatchTypes dispatchType = DispatchTypes.UnSet;
            dispatchType = EnumHelper.GetItem<DispatchTypes>(dispatchTypeString, DispatchTypes.UnSet);

            string queryTimeSpanName = RequestHelper.GetValue("queryTimeSpanName");

            DateTime queryTimeSpanValueStart = RequestHelper.GetValue("queryTimeSpanValueStart", DateTimeHelper.Min);
            DateTime queryTimeSpanValueEnd = RequestHelper.GetValue("queryTimeSpanValueEnd", DateTimeHelper.Min);

            //1.0获取部分（或者所有）部门内人员的集合
            List<BusinessUser> employeeList = new List<BusinessUser>();
            if (GuidHelper.IsInvalidOrEmpty(resourceValue) == false)
            {
                switch (resourceType.ToLower())
                {
                    case "e":
                        BusinessUser employee = BusinessUserBLL.Get(new Guid(resourceValue));
                        if (employee != null && employee.IsEmpty == false)
                        {
                            employeeList.Add(employee);
                        }
                        break;
                    case "d":
                    default:
                        string departmentFullPath = BusinessDepartmentBLL.Instance.Get(new Guid(resourceValue)).DepartmentFullPath;
                        employeeList = BusinessUserBLL.GetUsersByDepartment(departmentFullPath, true);
                        break;
                }
            }
            else
            {
                employeeList = BusinessUserBLL.GetList(UserTypes.Manager);
            }

            //1.1对获取的后的人员信息进行整理
            employeeList.Sort((x, y) =>
            {
                return x.DepartmentFullPath.CompareTo(y.DepartmentFullPath);
            });

            for (int i=employeeList.Count-1;i>=0;i--)
            {
                var item = employeeList[i];
                if (item.UserStatus != UserStatuses.Normal)
                {
                    employeeList.RemoveAt(i);
                }
            }

            //2.基于人员集合构建展示数据的存储结构
            Dictionary<Guid, EmployeeScoreStatisticalEntity> employeeDictionary = new Dictionary<Guid, EmployeeScoreStatisticalEntity>();
            for (int i = 0; i < employeeList.Count; i++)
            {
                BusinessUser employee = employeeList[i];
                EmployeeScoreStatisticalEntity employeeScoreStatisticalEntity = new EmployeeScoreStatisticalEntity();
                employeeScoreStatisticalEntity.EmployeeGuid = employee.UserGuid;
                employeeScoreStatisticalEntity.EmployeeName = employee.UserNameCN;
                employeeScoreStatisticalEntity.DepartmentGuid = employee.DepartmentGuid;
                employeeScoreStatisticalEntity.DepartmentFullName = employee.DepartmentFullPath;
                employeeScoreStatisticalEntity.DepartmentName = employee.DepartmentName;

                employeeDictionary[employee.UserGuid] = employeeScoreStatisticalEntity;
            }

            //3.获取限定条件内劳务人员的数据
            string sqlClause = String.Empty;
            switch (queryTimeSpanName)
            {

                case "jobStartingTime":
                    break;
                case "jobLeavingTime":
                    break;
                case "balanceTime":
                default:
                    sqlClause = @" select  Biz.SalarySummaryGuid as BizGuid,
		                                Biz.EnterpriseKey as EnterpriseKey, 
		                                GE.CompanyName as EnterpriseName,
		                                CU.UserGuid as LaborGuid,
		                                CU.UserNameCN as LaborName,
		                                LB.BusinessUserGuid as LBBusinessUserGuid,
		                                LB.BusinessUserName as LBBusinessUserName,
		                                LB.ServiceUserGuid as LBServiceUserGuid,
		                                LB.ServiceUserName as LBServiceUserName,
		                                ES.ProviderUserGuid as ESProviderUserGuid,
		                                ES.ProviderUserName as ESProviderUserName,
		                                ES.BusinessUserGuid as ESBusinessUserGuid,
		                                ES.BusinessUserName as ESBusinessUserName
                                from XQYCSalarySummary Biz Left Join CoreUser CU ON Biz.LaborKey= CU.UserGuid 
						                                   Left Join XQYCLabor LB ON Biz.LaborKey=LB.UserGuid  
						                                   Left Join GeneralEnterprise GE On Biz.EnterpriseKey = GE.EnterpriseGuid
						                                   Left Join XQYCEnterpriseService ES ON GE.EnterpriseGuid = ES.EnterpriseGuid
                                where ES.EnterpriseServiceType= 1 "; //--1表示劳务派遣合作关系
                    if (queryTimeSpanValueStart != DateTimeHelper.Min)
                    {
                        sqlClause += string.Format(" AND EnterpriseManageFeeCashDate >= '{0}' ", queryTimeSpanValueStart);
                    }

                    if (queryTimeSpanValueEnd != DateTimeHelper.Min)
                    {
                        sqlClause += string.Format(" AND EnterpriseManageFeeCashDate <= '{0}' ", queryTimeSpanValueEnd);
                    }

                    if (dispatchType != DispatchTypes.UnSet)
                    {
                        sqlClause += string.Format(" AND LB.CurrentDispatchType = {0} ", (int)dispatchType);
                    }

                    if (GuidHelper.IsInvalidOrEmpty(enterpriseKey) == false)
                    {
                        sqlClause += string.Format(" AND AND Biz.EnterpriseKey = '{0}' ", enterpriseKey);
                    }

                    break;
            }

            //4.对获取出来的劳务人员信息进行分类计数
            CalculateLaborCount(sqlClause, employeeDictionary);

            bool isExportExcel = RequestHelper.GetValue("exportExcel", false);
            if (isExportExcel == true)
            {
                List<EmployeeScoreStatisticalEntity> list = new List<EmployeeScoreStatisticalEntity>();
                foreach (KeyValuePair<Guid, EmployeeScoreStatisticalEntity> kvp in employeeDictionary)
                {
                   list.Add( kvp.Value);
                }
                return StatisticalSummaryToExcelFile(list);
            }
            else
            {
                return View(employeeDictionary);
            }
        }

        private ActionResult StatisticalSummaryToExcelFile(IList<EmployeeScoreStatisticalEntity> laborList)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["DepartmentFullName"] = "部门";
            dic["EmployeeName"] = "人员姓名";
            dic["LaborCountOfLaborBusiness"] = "招聘数";
            dic["LaborCountOfEnterpriseProvide"] = "信息提供数";
            dic["LaborCountOfEnterpriseBusiness"] = "开发数";
            dic["LaborCountOfLaborService"] = "客服数";

            Stream excelStream = ExcelHelper.WriteExcel(laborList, dic);
            return File(excelStream, ContentTypes.GetContentType("xls"), string.Format("劳务人员统计汇总-{0}.xls", DateTime.Now.ToShortDateString()));
        }

        private void CalculateLaborCount(string sqlClause, Dictionary<Guid, EmployeeScoreStatisticalEntity> employeeDictionary)
        {
            using (SqlDataReader reader = CommanHelperInstance.ExecuteReader(sqlClause))
            {
                while (reader.Read())
                {
                    Guid laborGuid = DataReaderHelper.GetFiledValue<Guid>(reader, "LaborGuid");
                    Guid LBBusinessUserGuid = DataReaderHelper.GetFiledValue<Guid>(reader, "LBBusinessUserGuid");
                    Guid LBServiceUserGuid = DataReaderHelper.GetFiledValue<Guid>(reader, "LBServiceUserGuid");
                    Guid ESProviderUserGuid = DataReaderHelper.GetFiledValue<Guid>(reader, "ESProviderUserGuid");
                    Guid ESBusinessUserGuid = DataReaderHelper.GetFiledValue<Guid>(reader, "ESBusinessUserGuid");

                    if (employeeDictionary.ContainsKey(LBBusinessUserGuid))
                    {
                        employeeDictionary[LBBusinessUserGuid].LaborCountOfLaborBusiness++;
                    }

                    if (employeeDictionary.ContainsKey(LBServiceUserGuid))
                    {
                        employeeDictionary[LBServiceUserGuid].LaborCountOfLaborService++;
                    }

                    if (employeeDictionary.ContainsKey(ESProviderUserGuid))
                    {
                        employeeDictionary[ESProviderUserGuid].LaborCountOfEnterpriseProvide++;
                    }

                    if (employeeDictionary.ContainsKey(ESBusinessUserGuid))
                    {
                        employeeDictionary[ESBusinessUserGuid].LaborCountOfEnterpriseBusiness++;
                    }
                }
            }
        }


        #endregion

        #region 辅助方法
        private CommonHelperEx<SqlTransaction, SqlConnection, SqlCommand, SqlDataReader, SqlParameter> CommanHelperInstance
        {
            get
            {
                return CommonHelperEx<SqlTransaction, SqlConnection, SqlCommand, SqlDataReader, SqlParameter>.Instance;
            }
        }


        #endregion
    }
}
