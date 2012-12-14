using System.Collections.Generic;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Setting;

namespace XQYC.Web.Models
{
    public class SystemConst
    {
        static SystemConst()
        {
            BasicSettingBLL.Instance.BasicSettingChanged += Instance_BasicSettingChanged;
        }

        static void Instance_BasicSettingChanged(object sender, DataForChange<BasicSettingEntity> args)
        {
            if (args.NewData.SettingKey == "CountPerPage")
            {
                countPerPage = Converter.ChangeType(args.NewData.SettingValue, 10);
            }

            if (args.NewData.SettingKey == "CountPerPageForLaborList")
            {
                countPerPageForLaborList = Converter.ChangeType(args.NewData.SettingValue, 10);
            }

            if (args.NewData.SettingKey == "MaxEnterpriseCountOfManager")
            {
                maxEnterpriseCountOfManager = Converter.ChangeType(args.NewData.SettingValue, 10);
            }
        }

        public const string PermissionItemValuePrefix = "SubModuleGuid::";
        public const string PermissionItemGuidValueSeperator = "||";

        public const string RoleItemValuePrefix = "RoleItemGuid::";
        public const string RoleItemValueSeperator = "||";

        private static int countPerPage = 0;
        /// <summary>
        /// 每列表页面显示的信息条目数量
        /// </summary>
        public static int CountPerPage
        {
            get
            {
                if (countPerPage == 0)
                {
                    countPerPage = Config.GetAppSettingInt("CountPerPage", 0);
                }

                if (countPerPage == 0)
                {
                    countPerPage = Converter.ChangeType(BasicSettingBLL.Instance.GetBySettingKey("CountPerPage").SettingValue, 10);
                }

                return countPerPage;
            }
        }

        private static int countPerPageForLaborList = 0;
        /// <summary>
        /// 每列表页面显示的信息条目数量(专门为劳务人员列表)
        /// </summary>
        public static int CountPerPageForLaborList
        {
            get
            {
                if (countPerPageForLaborList == 0)
                {
                    countPerPageForLaborList = Config.GetAppSettingInt("CountPerPageForLaborList", 0);
                }

                if (countPerPageForLaborList == 0)
                {
                    countPerPageForLaborList = Converter.ChangeType(BasicSettingBLL.Instance.GetBySettingKey("CountPerPageForLaborList").SettingValue, 10);
                }

                return countPerPageForLaborList;
            }
        }

        private static int maxEnterpriseCountOfManager = 0;
        /// <summary>
        /// 配置表中，费用项列表
        /// </summary>
        public static int MaxEnterpriseCountOfManager
        {
            get
            {
                if (maxEnterpriseCountOfManager == 0)
                {
                    maxEnterpriseCountOfManager = Converter.ChangeType(BasicSettingBLL.Instance.GetBySettingKey("MaxEnterpriseCountOfManager").SettingValue, 5);
                }

                return maxEnterpriseCountOfManager;
            }
        }

        private static string initialUserPassword = string.Empty;
        /// <summary>
        /// 管理员创建用户时，用户的初始密码
        /// </summary>
        public static string InitialUserPassword
        {
            get
            {
                if (string.IsNullOrWhiteSpace(initialUserPassword))
                {
                    initialUserPassword = Config.GetAppSetting("InitialUserPassword", "123");
                }

                return initialUserPassword;
            }
        }
    }
}