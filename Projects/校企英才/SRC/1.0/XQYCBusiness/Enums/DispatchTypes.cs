using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    /// <summary>
    /// 用工方式类型
    /// </summary>
    public enum DispatchTypes
    {
        /// <summary>
        /// 未设置
        /// </summary>
        [EnumItemDescription("zh-CN", "未设置")]
        UnSet=0,

        /// <summary>
        /// 劳务派遣
        /// </summary>
        [EnumItemDescription("zh-CN", "劳务派遣")]
        CommonDispatch=10,

        /// <summary>
        /// 转派遣
        /// </summary>
        [EnumItemDescription("zh-CN", "转派遣")]
        ShiftDispatch = 20,

        /// <summary>
        /// 代理招聘
        /// </summary>
        [EnumItemDescription("zh-CN", "代理招聘")]
        HireBroke=30,
    }
}
