using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiLand.General.BLL;
using HiLand.Utility.Data;
using HiLand.Utility.Setting;

namespace HiLand.Project.SiteWeb.Models
{
    public class ConfigConst
    {
        private static string companyName = string.Empty;
        public static string CompanyName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(companyName))
                {
                    companyName = Config.GetAppSetting("CompanyName");
                }

                return companyName;
            }
        }

        private static string companyNameShort = string.Empty;
        public static string CompanyNameShort
        {
            get
            {
                if (string.IsNullOrWhiteSpace(companyNameShort))
                {
                    companyNameShort = Config.GetAppSetting("CompanyNameShort");
                }

                return companyNameShort;
            }
        }

        private static int countPerPageForManage = 0;
        /// <summary>
        /// 每列表页面显示的信息条目数量(后台管理)
        /// </summary>
        public static int CountPerPageForManage
        {
            get
            {
                if (countPerPageForManage == 0)
                {
                    countPerPageForManage = Converter.ChangeType(BasicSettingBLL.Instance.GetBySettingKey("CountPerPageForManage").SettingValue, 10);
                }
                return countPerPageForManage;
            }
        }

        private static int countPerPageForEndUser = 0;
        /// <summary>
        /// 每列表页面显示的信息条目数量(前台显示)
        /// </summary>
        public static int CountPerPageForEndUser
        {
            get
            {
                if (countPerPageForEndUser == 0)
                {
                    countPerPageForEndUser = Converter.ChangeType(BasicSettingBLL.Instance.GetBySettingKey("CountPerPageForEndUser").SettingValue, 10);
                }
                return countPerPageForEndUser;
            }
        }
    }
}