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
    public class DepartmentController : Controller
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<BusinessDepartment> list = BusinessDepartmentBLL.Instance.GetOrdedList(Logics.False, string.Empty);
            return View(list);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        public ActionResult Item(string keyGuid = StringHelper.Empty, string parentKey=StringHelper.Empty)
        {
            BusinessDepartment department = BusinessDepartment.Empty;
            if (string.IsNullOrWhiteSpace(keyGuid) == false)
            {
                department = BusinessDepartmentBLL.Instance.Get(keyGuid);
            }

            this.ViewBag.ParentKey = parentKey;

            return View(department);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Item(string keyGuid, string parentKey, BusinessDepartment entity, bool isOnlyPlaceHolder = true)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;

            BusinessDepartment department = null;
            if (GuidHelper.IsInvalidOrEmpty(keyGuid))
            {
                department = new BusinessDepartment();
                department.DepartmentGuid = GuidHelper.NewGuid();
                department.DepartmentParentGuid = Converter.TryToGuid(parentKey);
                SetTargetEntityValue(entity, ref department);
                isSuccessful = BusinessDepartmentBLL.Instance.Create(department);
            }
            else
            {
                department = BusinessDepartmentBLL.Instance.Get(keyGuid);
                SetTargetEntityValue(entity,ref department);
                isSuccessful = BusinessDepartmentBLL.Instance.Update(department);
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
        private static void SetTargetEntityValue(BusinessDepartment originalEntity,ref BusinessDepartment targetEntity)
        {
            targetEntity.DepartmentName = originalEntity.DepartmentName;
            targetEntity.DepartmentNameShort = originalEntity.DepartmentNameShort;
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.DepartmentType = originalEntity.DepartmentType;
        }

        public ActionResult Permission(string departmentGuid, string departmentName = "")
        {
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                departmentName = BusinessDepartmentBLL.Instance.Get(departmentGuid).DepartmentName;
            }

            string returnUrl = RequestHelper.CurrentRequest.AppRelativeCurrentExecutionFilePath;
            return RedirectToAction("Index", "Permission", new { ownerGuid = departmentGuid, ownerName = departmentName, ownerType = ExecutorTypes.Department, permissionMode = PermissionModes.Allow, returenUrl = returnUrl });
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public ActionResult Delete(string itemKey)
        {
            BusinessDepartmentBLL.Instance.Delete(itemKey);
            string url = RequestHelper.GetValue("returnUrl");
            bool isUsingCompress = RequestHelper.GetValue<bool>("isUsingCompress");
            if (isUsingCompress == true)
            {
                url = CompressHelper.Decompress(url);
            }
            return Redirect(url);
        }
    }
}
