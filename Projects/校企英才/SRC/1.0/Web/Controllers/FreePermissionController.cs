using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Web;
using XQYC.Business.BLL;
using XQYC.Business.Entity;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    /// <summary>
    /// 所有不用权限控制的逻辑
    /// </summary>
    public class FreePermissionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region 企业部分
        /// <summary>
        /// 根据企业控件的信息，获取企业合同列表
        /// </summary>
        /// <param name="enterpriseControlID"></param>
        /// <returns></returns>
        public ActionResult EnterpriseContractItemList(string enterpriseControlID)
        {
            Guid enterpriserGuid = RequestHelper.GetValue<Guid>(enterpriseControlID);
            List<EnterpriseContractEntity> contractList = EnterpriseContractBLL.Instance.GetList(string.Format(" EnterpriseGuid='{0}' AND ContractStatus={1} ", enterpriserGuid, (int)Logics.True));
            CascadingCollection coll = new CascadingCollection();
            foreach (EnterpriseContractEntity currentItem in contractList)
            {
                coll.AddItem(currentItem.ContractTitle, currentItem.ContractGuid.ToString());
            }

            return Json(coll);
        }
        #endregion

        #region 劳务人员部分
        
        #endregion

        #region 角色部分
        /// <summary>
        /// 角色选择器
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public ActionResult RoleSelector(string userKey)
        {
            Guid userGuid = Converter.TryToGuid(userKey);
            List<BusinessRole> allRoleList = BusinessRoleBLL.GetList(Logics.True, string.Empty);
            List<BusinessRole> userRoleList = BusinessUserBLL.GetUserRoles(userGuid);

            this.ViewBag.UserKey = userKey;
            this.ViewBag.UserRoleList = userRoleList;

            return View(allRoleList);
        }

        /// <summary>
        /// 角色选择器
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleSelector(string userKey, bool isOnlyPlaceHolder = true)
        {
            Guid userGuid = Converter.TryToGuid(userKey);

            if (userGuid == Guid.Empty)
            {
                return Json(new LogicStatusInfo(false, "保存权限失败"));
            }
            else
            {
                List<Guid> roleGuidList = new List<Guid>();

                NameValueCollection nvc = this.Request.Form;
                //获取选中的checkbox
                for (int i = 0; i < nvc.Count; i++)
                {
                    string currentKey = nvc.AllKeys[i];
                    string currentValue = nvc[i];

                    if (currentKey.StartsWith(SystemConst.RoleItemValuePrefix) && currentValue.ToLower() == "on")
                    {
                        int seperatorPos = currentKey.LastIndexOf(SystemConst.PermissionItemGuidValueSeperator);
                        int valueStarPos = seperatorPos + SystemConst.PermissionItemGuidValueSeperator.Length;
                        string currentRoleString = currentKey.Substring(valueStarPos);
                        Guid currentRoleGuid = Converter.TryToGuid(currentRoleString);
                        if (currentRoleGuid != Guid.Empty)
                        {
                            roleGuidList.Add(currentRoleGuid);
                        }
                    }
                }


                BusinessUserBLL.UpdateUserRoles(userGuid, roleGuidList);

                return Json(new LogicStatusInfo(true, "保存权限成功"));
            }
        }
        #endregion
    }
}
