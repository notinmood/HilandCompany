using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    /// <summary>
    /// 劳务人员来源类型
    /// </summary>
    public enum ComeFromTypes
    {
        /// <summary>
        /// 未设置
        /// </summary>
        [EnumItemDescription("zh-CN", "未设置")]
        Unset = 0,

        /// <summary>
        /// 后台录入
        /// </summary>
        [EnumItemDescription("zh-CN", "后台录入")]
        ManageWrite = 1,

        /// <summary>
        /// 网站注册
        /// </summary>
        [EnumItemDescription("zh-CN", "网站注册")]
        WebRegister = 5,

        /// <summary>
        /// 手机注册
        /// </summary>
        [EnumItemDescription("zh-CN", "手机注册")]
        MobileRegister = 6,
    }
}
