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

        //private static List<BasicSettingEntity> costList = null;
        ///// <summary>
        ///// 配置表中，费用项列表
        ///// </summary>
        //public static List<BasicSettingEntity> CostList
        //{
        //    get
        //    {
        //        if (costList == null)
        //        {
        //            costList = BasicSettingBLL.Instance.GetListByCategory("CostItem");
        //        }

        //        return costList;
        //    }
        //}

        private static string initialUserPassword = string.Empty;
        /// <summary>
        /// 管理员创建用户时，用户的初始密码
        /// </summary>
        public static string InitialUserPassword
        {
            get
            {
                if (string.IsNullOrWhiteSpace( initialUserPassword))
                {
                    initialUserPassword = Config.GetAppSetting("InitialUserPassword", "123");
                }

                return initialUserPassword;
            }
        }
    }
}