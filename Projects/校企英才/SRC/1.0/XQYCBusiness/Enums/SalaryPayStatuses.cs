using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    /// <summary>
    /// 薪资支付状态
    /// </summary>
    public enum SalaryPayStatuses
    {
        /// <summary>
        /// 企业待付
        /// </summary>
        [EnumItemDescription("zh-CN", "企业待付")]
        NeedPay =1,

        /// <summary>
        /// 企业已付（企业已将费用付给劳务中介机构）
        /// </summary>
        [EnumItemDescription("zh-CN", "企业已付")]
        PaidToOrgnization=5,

        /// <summary>
        /// 已经付给员工（劳务中介机构已经将费用付给了员工）
        /// </summary>
        [EnumItemDescription("zh-CN", "已付人员")]
        PaidToPeople= 8,
    }
}
