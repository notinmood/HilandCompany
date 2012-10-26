using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework4.Permission;
using HiLand.Framework4.Permission.Attributes;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Paging;
using HiLand.Utility.Web;
using HiLand.Utility4.MVC.Controls;
using Webdiyer.WebControls.Mvc;
using XQYC.Business.BLL;
using XQYC.Business.Entity;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    /// <summary>
    /// 信息员管理
    /// </summary>
    [PermissionAuthorize]
    public class InformationBrokerController : Controller
    {
        public ActionResult Index(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("Index", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
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
            whereClause += " ) ";
            //--end--------------------------------------------------------------------------
            string orderClause = "InformationBrokerID DESC";

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            PagedEntityCollection<InformationBrokerEntity> entityList = InformationBrokerBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<InformationBrokerEntity> pagedExList = new PagedList<InformationBrokerEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }

        public ActionResult Item(string itemKey)
        {
            InformationBrokerEntity department = InformationBrokerEntity.Empty;
            if (string.IsNullOrWhiteSpace(itemKey) == false)
            {
                department = InformationBrokerBLL.Instance.Get(itemKey);
            }

            return View(department);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Item(string itemKey, InformationBrokerEntity entity, bool isOnlyPlaceHolder = true)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            string returnUrl = RequestHelper.GetValue("returnUrl");
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index");
            }

            InformationBrokerEntity targetEntity = null;
            if (string.IsNullOrWhiteSpace(itemKey))
            {
                targetEntity = new InformationBrokerEntity();
                SetTargetEntityValue(entity, ref targetEntity);
                targetEntity.CreateUserKey = BusinessUserBLL.CurrentUserGuid.ToString();
                targetEntity.CreateDate = DateTime.Now;
                isSuccessful = InformationBrokerBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = InformationBrokerBLL.Instance.Get(itemKey);
                SetTargetEntityValue(entity, ref targetEntity);

                isSuccessful = InformationBrokerBLL.Instance.Update(targetEntity);
            }


            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功";
            }
            else
            {
                displayMessage = "数据保存失败";
            }

            return Redirect(returnUrl);
        }

        /// <summary>
        /// 通过一个实体给另外一个实体赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        private static void SetTargetEntityValue(InformationBrokerEntity originalEntity, ref InformationBrokerEntity targetEntity)
        {
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.ContactPerson = originalEntity.ContactPerson;
            targetEntity.InformationBrokerName = originalEntity.InformationBrokerName;
            targetEntity.InformationBrokerNameShort = originalEntity.InformationBrokerNameShort;
            targetEntity.PrincipleAddress = originalEntity.PrincipleAddress;
            targetEntity.InformationBrokerWWW = originalEntity.InformationBrokerWWW;
            targetEntity.InformationBrokerType = originalEntity.InformationBrokerType;
            targetEntity.Email = originalEntity.Email;
            targetEntity.InformationBrokerLevel = originalEntity.InformationBrokerLevel;
            targetEntity.InformationBrokerMemo = originalEntity.InformationBrokerMemo;
            targetEntity.InformationBrokerDescription = originalEntity.InformationBrokerDescription;
            targetEntity.Fax = originalEntity.Fax;
            targetEntity.IndustryKey = originalEntity.IndustryKey;
            targetEntity.PostCode = originalEntity.PostCode;
            targetEntity.InformationBrokerKind = originalEntity.InformationBrokerKind;
            targetEntity.Telephone = originalEntity.Telephone;
            targetEntity.AreaCode = originalEntity.AreaCode;
            targetEntity.FinanceUserName = RequestHelper.GetValue("FinanceUser");
            targetEntity.FinanceUserGuid = RequestHelper.GetValue<Guid>("FinanceUser_Value");
            targetEntity.ProviderUserName = RequestHelper.GetValue("ProviderUser");
            targetEntity.ProviderUserGuid = RequestHelper.GetValue<Guid>("ProviderUser_Value");
            targetEntity.RecommendUserName = RequestHelper.GetValue("RecommendUser");
            targetEntity.RecommendUserGuid = RequestHelper.GetValue<Guid>("RecommendUser_Value");
            targetEntity.ServiceUserName = RequestHelper.GetValue("ServiceUser");
            targetEntity.ServiceUserGuid = RequestHelper.GetValue<Guid>("ServiceUser_Value");
        }

        public ActionResult UserList(string itemKey, string itemName = StringHelper.Empty)
        {
            List<BusinessUser> userList = null;

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format("EnterpriseKey='{0}'", itemGuid.ToString());
                userList = BusinessUserBLL.GetList(whereClause);
            }

            this.ViewBag.EnterpriseKey = itemKey;
            this.ViewBag.EnterpriseName = itemName;
            return View(userList);
        }

        public ActionResult UserItem(string enterpriseKey, string itemKey = StringHelper.Empty)
        {
            BusinessUser user = BusinessUser.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                user = BusinessUserBLL.Get(new Guid(itemKey));
            }

            this.ViewBag.EnterpriseKey = enterpriseKey;

            return View(user);
        }

        [HttpPost]
        public ActionResult UserItem(string enterpriseKey, string itemKey, BusinessUser originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            BusinessUser targetUser = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetUser = new BusinessUser();

                targetUser.EnterpriseKey = enterpriseKey;
                targetUser.UserType = UserTypes.EnterpriseUser;
                targetUser.Password = SystemConst.InitialUserPassword;

                SetTargetUserEntityValue(originalEntity, ref  targetUser);

                CreateUserRoleStatuses createStatus = CreateUserRoleStatuses.Successful;
                BusinessUserBLL.CreateUser(targetUser, out createStatus);
                if (createStatus == CreateUserRoleStatuses.Successful)
                {
                    isSuccessful = true;
                }
            }
            else
            {
                targetUser = BusinessUserBLL.Get(new Guid(itemKey));

                SetTargetUserEntityValue(originalEntity, ref  targetUser);
                isSuccessful = BusinessUserBLL.UpdateUser(targetUser);
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

        private void SetTargetUserEntityValue(BusinessUser originalEntity, ref BusinessUser targetEntity)
        {
            targetEntity.UserName = originalEntity.UserName;
            targetEntity.UserNameCN = originalEntity.UserNameCN;
            targetEntity.UserSex = originalEntity.UserSex;
            targetEntity.UserMobileNO = originalEntity.UserMobileNO;
            targetEntity.UserStatus = originalEntity.UserStatus;
        }

        public ActionResult Password(string userGuid, string userName = "")
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = EmployeeBLL.Instance.Get(userGuid).UserName;
            }

            string returnUrl = RequestHelper.CurrentRequest.AppRelativeCurrentExecutionFilePath;
            return RedirectToAction("_ChangePasswordForManage", "Home", new { area = "UserCenter", userGuid = userGuid, userName = userName, returenUrl = returnUrl });
        }

        /// <summary>
        /// 信息员自动完成的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AutoCompleteData()
        {
            string userValueInputted = RequestHelper.GetValue("term");
            List<AutoCompleteEntity> itemList = new List<AutoCompleteEntity>();
            string whereClause = string.Format(" ( InformationBrokerName like '{0}%' OR InformationBrokerNameShort like '{0}%' ) AND  CanUsable={1}", userValueInputted, (int)Logics.True);
            List<InformationBrokerEntity> userList = InformationBrokerBLL.Instance.GetList(whereClause);

            foreach (InformationBrokerEntity currentLabor in userList)
            {
                AutoCompleteEntity item = new AutoCompleteEntity();
                item.details = "nothing";
                item.key = currentLabor.InformationBrokerGuid.ToString();
                item.label = string.Format("{0}({1})", currentLabor.InformationBrokerName, currentLabor.InformationBrokerNameShort);
                item.value = currentLabor.InformationBrokerName;

                itemList.Add(item);
            }

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
    }
}
