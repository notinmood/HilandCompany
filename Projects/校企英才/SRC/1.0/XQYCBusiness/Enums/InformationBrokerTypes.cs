using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    /// <summary>
    /// 信息员类型
    /// </summary>
    public enum InformationBrokerTypes
    {
        /// <summary>
        /// 个人
        /// </summary>
        [EnumItemDescription("zh-CN", "个人")]
        Individual=0,

        /// <summary>
        /// 中介
        /// </summary>
        [EnumItemDescription("zh-CN", "中介")]
        CommonBroker=1,

        /// <summary>
        /// 学校
        /// </summary>
        [EnumItemDescription("zh-CN", "学校")]
        School=2,
    }
}
