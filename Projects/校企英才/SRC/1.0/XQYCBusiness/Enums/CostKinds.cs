using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    public enum CostKinds
    {
        /// <summary>
        /// 保险
        /// </summary>
        [EnumItemDescription("zh-CN", "保险")]
        Insurance = 1,

        /// <summary>
        /// 公积金
        /// </summary>
        [EnumItemDescription("zh-CN", "公积金")]
        ReserveFund = 2,

        /// <summary>
        /// 管理费
        /// </summary>
        [EnumItemDescription("zh-CN", "管理费")]
        ManageFee=3,

        /// <summary>
        /// 混合费用
        /// </summary>
        /// <remarks>
        /// （企业付给的混合的费用，其内可以包括保险，公积金，管理费等）
        /// </remarks>
        [EnumItemDescription("zh-CN", "混合费用")]
        MixCost=10,
    }
}
