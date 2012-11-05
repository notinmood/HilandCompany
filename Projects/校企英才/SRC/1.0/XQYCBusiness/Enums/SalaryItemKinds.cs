using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    public enum SalaryItemKinds
    {
        /// <summary>
        /// 基本薪资
        /// </summary>
        [EnumItemDescription("zh-CN", "基本薪资")]
        BasicSalary=1,

        /// <summary>
        /// 各种奖金
        /// </summary>
        [EnumItemDescription("zh-CN", "奖金")]
        Rewards= 2,

        /// <summary>
        /// 各种扣费
        /// </summary>
        [EnumItemDescription("zh-CN", "扣费")]
        Rebate= 11,

        /// <summary>
        /// 保险
        /// </summary>
        [EnumItemDescription("zh-CN", "保险")]
        Insurance= 30,

        /// <summary>
        /// 保险(企业部分)
        /// </summary>
        [EnumItemDescription("zh-CN", "保险(企业部分)")]
        InsuranceEnterprise = 31,

        /// <summary>
        /// 保险(个人部分)
        /// </summary>
        [EnumItemDescription("zh-CN", "保险(个人部分)")]
        InsurancePersonal = 32,

        /// <summary>
        /// 公积金
        /// </summary>
        [EnumItemDescription("zh-CN", "公积金")]
        ReserveFund=50,

        /// <summary>
        /// 公积金(企业部分)
        /// </summary>
        [EnumItemDescription("zh-CN", "公积金(企业部分)")]
        ReserveFundEnterprise = 51,

        /// <summary>
        /// 公积金(个人部分)
        /// </summary>
        [EnumItemDescription("zh-CN", "公积金(个人部分)")]
        ReserveFundPersonal = 52,

        /// <summary>
        /// 管理费
        /// </summary>
        [EnumItemDescription("zh-CN", "管理费")]
        ManageFee=60,
        
        /// <summary>
        /// 工资税
        /// </summary>
        [EnumItemDescription("zh-CN", "工资税")]
        SalaryTax= 80,
    }
}
