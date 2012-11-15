using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.BusinessCore.Enum;
using HiLand.Framework.Membership;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Setting.SectionHandler;
using HiLand.Utility4.MVC.SectionHandler;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    /// <summary>
    /// 权限设置
    /// </summary>
    public class PermissionController : Controller
    {
        /// <summary>
        /// 操作权限设置
        /// </summary>
        /// <param name="ownerGuid"></param>
        /// <param name="ownerName"></param>
        /// <param name="ownerType"></param>
        /// <param name="permissionMode"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Index(string ownerGuid = "b378663f-c02a-4205-957d-e47ec331d535", string ownerName = "xieran", ExecutorTypes ownerType = ExecutorTypes.User, PermissionModes permissionMode = PermissionModes.Allow, string returnUrl = StringHelper.Empty)
        {
            PermissionValidateConfig config = PermissionValidateConfig.GetConfig();

            this.ViewBag.OwnerGuid = ownerGuid;
            this.ViewBag.OwnerName = ownerName;
            this.ViewBag.OwnerType = ownerType;
            this.ViewBag.PermissionMode = permissionMode;
            this.ViewBag.ReturnUrl = returnUrl;
            return View(config);
        }

        /// <summary>
        /// 操作权限设置
        /// </summary>
        /// <param name="ownerGuid"></param>
        /// <param name="isOnlyPlaceHolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string ownerGuid, ExecutorTypes ownerType, PermissionModes permissionMode)
        {
            NameValueCollection nvc = this.Request.Form;
            Dictionary<Guid, PermissionItem> changedPermissionItems = new Dictionary<Guid, PermissionItem>();
            //获取选中的checkbox
            for (int i = 0; i < nvc.Count; i++)
            {
                string currentKey = nvc.AllKeys[i];
                string currentValue = nvc[i];

                if (currentKey.StartsWith(SystemConst.PermissionItemValuePrefix) && currentValue.ToLower() == "on")
                {
                    int guidStartPos = SystemConst.PermissionItemValuePrefix.Length;
                    int seperatorPos = currentKey.LastIndexOf(SystemConst.PermissionItemGuidValueSeperator);
                    int valueStarPos = seperatorPos + SystemConst.PermissionItemGuidValueSeperator.Length;
                    string permissionItemGuidString = currentKey.Substring(guidStartPos, seperatorPos - guidStartPos);
                    Guid permissionItemGuid = new Guid(permissionItemGuidString);
                    string permissionItemValueString = currentKey.Substring(valueStarPos);
                    int permissionItemValue = 0;
                    bool isSuccessful = int.TryParse(permissionItemValueString, out permissionItemValue);
                    if (isSuccessful == true)
                    {
                        if (changedPermissionItems.ContainsKey(permissionItemGuid))
                        {
                            changedPermissionItems[permissionItemGuid].PermissionItemValue |= permissionItemValue;
                        }
                        else
                        {
                            PermissionItem permissionItem =
                                new PermissionItem(permissionItemGuid, permissionItemValue, BusinessUserBLL.CurrentUser.UserGuid, BusinessUserBLL.CurrentUser.UserType, Logics.False);
                            changedPermissionItems.Add(permissionItemGuid, permissionItem);
                        }
                    }
                }
            }

            foreach ( var permissionItem in changedPermissionItems)
            {
                BusinessPermission currentItem = new BusinessPermission(permissionItem.Value);
                currentItem.OwnerKey = ownerGuid.ToString();
                currentItem.OwnerType = ownerType;
                currentItem.PermissionMode = permissionMode;
                currentItem.PermissionKind = PermissionKinds.Operating;

                BusinessPermissionBLL.Instance.CreateOrUpdate(currentItem);
            }

            return Json(new LogicStatusInfo(true, "保存权限成功"));
        }


        /// <summary>
        /// 数据权限设置
        /// </summary>
        /// <param name="ownerGuid"></param>
        /// <param name="ownerName"></param>
        /// <param name="ownerType"></param>
        /// <param name="permissionMode"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Data(string ownerGuid = "b378663f-c02a-4205-957d-e47ec331d535", string ownerName = "xieran", ExecutorTypes ownerType = ExecutorTypes.User)
        {
            PermissionDataConfig config = PermissionDataConfig.GetConfig();

            this.ViewBag.OwnerGuid = ownerGuid;
            this.ViewBag.OwnerName = ownerName;
            this.ViewBag.OwnerType = ownerType;
            return View(config);
        }

        /// <summary>
        /// 数据权限设置
        /// </summary>
        /// <param name="ownerGuid"></param>
        /// <param name="isOnlyPlaceHolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Data(string ownerGuid, ExecutorTypes ownerType)
        {
            NameValueCollection nvc = this.Request.Form;
            Dictionary<Guid, PermissionItem> changedPermissionItems = new Dictionary<Guid, PermissionItem>();
            //获取选中的radio值
            for (int i = 0; i < nvc.Count; i++)
            {
                string currentKey = nvc.AllKeys[i];
                string currentValue = nvc[i];

                if (currentKey.StartsWith(SystemConst.PermissionItemValuePrefix) && currentKey.EndsWith("||0"))
                {
                    string settingName = currentKey.Substring(0,currentKey.Length-1);

                    int guidStartPos = SystemConst.PermissionItemValuePrefix.Length;
                    int seperatorPos = currentKey.LastIndexOf(SystemConst.PermissionItemGuidValueSeperator);
                    int valueStarPos = seperatorPos + SystemConst.PermissionItemGuidValueSeperator.Length;
                    string settingKeyString = currentKey.Substring(guidStartPos, seperatorPos - guidStartPos);
                    Guid settingkeyGuid = new Guid(settingKeyString);
                    string settingValue = nvc[settingName];
                    if (string.IsNullOrWhiteSpace(settingValue) == false)
                    {
                        int permissionItemValue;
                        bool isSuccessful = int.TryParse(settingValue, out permissionItemValue);
                        if (isSuccessful == true)
                        {
                            //changedPermissionItems[settingKey] = permissionItemValue;
                            if (changedPermissionItems.ContainsKey(settingkeyGuid))
                            {
                                changedPermissionItems[settingkeyGuid].PermissionItemValue = permissionItemValue;
                            }
                            else
                            {
                                PermissionItem permissionItem =
                                    new PermissionItem(settingkeyGuid, permissionItemValue, BusinessUserBLL.CurrentUser.UserGuid, BusinessUserBLL.CurrentUser.UserType, Logics.False);
                                changedPermissionItems.Add(settingkeyGuid, permissionItem);
                            }
                        }
                    }
                }
            }

            foreach (var permissionItem in changedPermissionItems)
            {
                BusinessPermission currentItem = new BusinessPermission(permissionItem.Value);
                currentItem.OwnerKey = ownerGuid.ToString();
                currentItem.OwnerType = ownerType;
                currentItem.PermissionMode =  PermissionModes.Allow;
                currentItem.PermissionKind = PermissionKinds.Data;

                BusinessPermissionBLL.Instance.CreateOrUpdate(currentItem);
            }

            return Json(new LogicStatusInfo(true, "保存权限成功"));
        }
    }
}
