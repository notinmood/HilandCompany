using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    /// <summary>
    /// 各种费用的种类（是企业担负还是个人担负）
    /// </summary>
    public enum CostTypes
    {
        /// <summary>
        /// 企业承担部分
        /// </summary>
        [EnumItemDescription("zh-CN", "企业承担部分")]
        Enterprise=0,

        /// <summary>
        /// 个人承担部分
        /// </summary>
        [EnumItemDescription("zh-CN", "个人承担部分")]
        Person=1,
    }
}
