using System;
using System.Web.Mvc;
using HiLand.Framework4.Permission;
using HiLand.Framework4.Permission.Attributes;
using HiLand.Utility.Enums;
using HiLand.Utility.Paging;
using HiLand.Utility.Web;
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
                targetEntity.Password = SystemConst.InitialUserPassword;
                targetEntity.UserType = UserTypes.Broker;
                targetEntity.UserRegisterDate = DateTime.Now;
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

            return Redirect(returnUrl); //Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }

        /// <summary>
        /// 通过一个实体给另外一个实体赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        private static void SetTargetEntityValue(InformationBrokerEntity originalEntity, ref InformationBrokerEntity targetEntity)
        {
            targetEntity.UserName = originalEntity.UserName;
            targetEntity.UserNameCN = originalEntity.UserNameCN;
            targetEntity.DepartmentGuid = originalEntity.DepartmentGuid;
            targetEntity.UserEmail = originalEntity.UserEmail;
            targetEntity.UserStatus = originalEntity.UserStatus;
            targetEntity.InformationBrokerStatus = originalEntity.UserStatus;
            targetEntity.UserSex = originalEntity.UserSex;
            targetEntity.FinanceUserName = RequestHelper.GetValue("FinanceUser");
            targetEntity.FinanceUserGuid = RequestHelper.GetValue<Guid>("FinanceUser_Value");
            targetEntity.ProviderUserName = RequestHelper.GetValue("ProviderUser");
            targetEntity.ProviderUserGuid = RequestHelper.GetValue<Guid>("ProviderUser_Value");
            targetEntity.RecommendUserName = RequestHelper.GetValue("RecommendUser");
            targetEntity.RecommendUserGuid = RequestHelper.GetValue<Guid>("RecommendUser_Value");
            targetEntity.ServiceUserName = RequestHelper.GetValue("ServiceUser");
            targetEntity.ServiceUserGuid = RequestHelper.GetValue<Guid>("ServiceUser_Value");
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
    }
}
