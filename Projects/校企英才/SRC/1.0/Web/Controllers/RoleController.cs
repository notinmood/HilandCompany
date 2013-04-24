using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.BusinessCore.Enum;
using HiLand.Framework4.Permission.Attributes;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Web;

namespace XQYC.Web.Controllers
{
    [PermissionAuthorize]
    public class RoleController : Controller
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<BusinessRole> list = BusinessRoleBLL.GetList(Logics.False, string.Empty);
            return View(list);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        public ActionResult Item(string keyGuid)
        {
            Guid targetGuid = Converter.TryToGuid(keyGuid);
            BusinessRole targetObject = BusinessRoleBLL.Get(targetGuid);

            return View(targetObject);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Item(string keyGuid, BusinessRole entity, bool isOnlyPlaceHolder = true)
        {
            Guid targetGuid = Converter.TryToGuid(keyGuid);
            bool isSuccessful = false;
            string displayMessage = string.Empty;

            BusinessRole targetRole = null;
            if (targetGuid == Guid.Empty)
            {
                targetRole = new BusinessRole();
                targetRole.RoleGuid = GuidHelper.NewGuid();

                SetTargetEntityValue(entity, ref targetRole);

                CreateUserRoleStatuses status;
                BusinessRoleBLL.CreateRole(targetRole, out status);

                if (status == CreateUserRoleStatuses.Successful)
                {
                    isSuccessful = true;
                }
                else
                {
                    isSuccessful = false;
                }
            }
            else
            {
                targetRole = BusinessRoleBLL.Get(targetGuid);

                SetTargetEntityValue(entity, ref targetRole);

                isSuccessful = BusinessRoleBLL.UpdateRole(targetRole);
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

        /// <summary>
        /// 通过一个实体给另外一个实体赋值
        /// </summary>
        /// <param name="originalEntity"></param>
        /// <param name="targetEntity"></param>
        private static void SetTargetEntityValue(BusinessRole originalEntity, ref BusinessRole targetEntity)
        {
            targetEntity.RoleDescrition = originalEntity.RoleDescrition;
            targetEntity.RoleName = originalEntity.RoleName;
            targetEntity.CanUsable = originalEntity.CanUsable;
        }

        /// <summary>
        /// 角色对应的权限
        /// </summary>
        /// <param name="targetKey"></param>
        /// <param name="targetName"></param>
        /// <returns></returns>
        public ActionResult Permission(string targetKey, string targetName = "")
        {
            Guid targetGuid = Converter.TryToGuid(targetKey);
            if (string.IsNullOrWhiteSpace(targetName))
            {
                targetName = BusinessRoleBLL.Get(targetGuid).RoleName;
            }

            string returnUrl = RequestHelper.CurrentRequest.AppRelativeCurrentExecutionFilePath;
            return RedirectToAction("Index", "Permission", new { ownerGuid = targetKey, ownerName = targetName, ownerType = ExecutorTypes.Role, permissionMode = PermissionModes.Allow, returenUrl = returnUrl });
        }
    }
}
