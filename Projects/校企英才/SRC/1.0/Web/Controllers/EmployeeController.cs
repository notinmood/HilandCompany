using System.Web.Mvc;
using HiLand.Framework.BusinessCore.Enum;
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
    /// 内部员工管理
    /// </summary>
    [PermissionAuthorize]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// 内部员工列表
        /// </summary>
        /// <returns></returns>
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
            string orderClause = "EmployeeID DESC";
            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");
            
            //如果未设置人员的状态，那么仅仅显示当前有效的用户
            if (whereClause.ToLower().Contains("userstatus") == false)
            {
                whereClause += string.Format(" AND UserStatus={0} ", (int)UserStatuses.Normal);
            }

            PagedEntityCollection<EmployeeEntity> entityList = EmployeeBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<EmployeeEntity> pagedExList = new PagedList<EmployeeEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        public ActionResult Item(string keyGuid)
        {
            EmployeeEntity employeeEntity = EmployeeEntity.Empty;
            if (string.IsNullOrWhiteSpace(keyGuid) == false)
            {
                employeeEntity = EmployeeBLL.Instance.Get(keyGuid);
            }

            return View(employeeEntity);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Item(string keyGuid, EmployeeEntity entity, bool isOnlyPlaceHolder = true)
        {
            bool isSuccessful = false;
            CreateUserRoleStatuses createStatus = CreateUserRoleStatuses.Successful;
            string displayMessage = string.Empty;

            EmployeeEntity targetEntity = null;
            if (string.IsNullOrWhiteSpace(keyGuid))
            {
                targetEntity = new EmployeeEntity();
                SetTargetEntityValue(entity, ref targetEntity);
                targetEntity.Password = SystemConst.InitialUserPassword;
                targetEntity.UserType = UserTypes.Manager;
                createStatus = EmployeeBLL.Instance.Create(targetEntity);
                if (createStatus == CreateUserRoleStatuses.Successful)
                {
                    isSuccessful = true;
                }
            }
            else
            {
                targetEntity = EmployeeBLL.Instance.Get(keyGuid);
                SetTargetEntityValue(entity, ref targetEntity);

                isSuccessful = EmployeeBLL.Instance.Update(targetEntity);
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

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }

        /// <summary>
        /// 通过一个实体给另外一个实体赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        private static void SetTargetEntityValue(EmployeeEntity originalEntity, ref EmployeeEntity targetEntity)
        {
            //targetEntity = originalEntity.Clone();
            targetEntity.UserName = originalEntity.UserName;
            targetEntity.UserNameCN = originalEntity.UserNameCN;
            targetEntity.DepartmentGuid = originalEntity.DepartmentGuid;
            targetEntity.UserEmail = originalEntity.UserEmail;
            targetEntity.UserStatus = originalEntity.UserStatus;
            targetEntity.Foo = originalEntity.Foo;
            targetEntity.DepartmentUserType = originalEntity.DepartmentUserType;
            targetEntity.UserBirthDay = originalEntity.UserBirthDay;

            targetEntity.UserSex = originalEntity.UserSex;
            targetEntity.UserTitle = originalEntity.UserTitle;
        }

        /// <summary>
        /// 用户的数据权限
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult PermissionData(string userGuid, string userName = "")
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = EmployeeBLL.Instance.Get(userGuid).UserName;
            }

            string returnUrl = RequestHelper.CurrentRequest.AppRelativeCurrentExecutionFilePath;
            return RedirectToAction("Data", "Permission", new { ownerGuid = userGuid, ownerName = userName, ownerType = ExecutorTypes.User });
        }

        /// <summary>
        /// 用户的允许权限
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult PermissionAllow(string userGuid, string userName = "")
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = EmployeeBLL.Instance.Get(userGuid).UserName;
            }

            string returnUrl = RequestHelper.CurrentRequest.AppRelativeCurrentExecutionFilePath;
            return RedirectToAction("Index", "Permission", new { ownerGuid = userGuid, ownerName = userName, ownerType = ExecutorTypes.User, permissionMode = PermissionModes.Allow, returenUrl = returnUrl });
        }

        /// <summary>
        /// 用户的拒绝权限
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult PermissionDeny(string userGuid, string userName = "")
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = EmployeeBLL.Instance.Get(userGuid).UserName;
            }

            string returnUrl = RequestHelper.CurrentRequest.AppRelativeCurrentExecutionFilePath;
            return RedirectToAction("Index", "Permission", new { ownerGuid = userGuid, ownerName = userName, ownerType = ExecutorTypes.User, permissionMode = PermissionModes.Deny, returenUrl = returnUrl });
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
        /// 角色选择器
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public ActionResult RoleSelector(string userKey)
        {
            return RedirectToAction("RoleSelector", "FreePermission", new { userKey = userKey }); ;
        }
    }
}
