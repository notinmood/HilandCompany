using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    /// <summary>
    /// 户口类型
    /// </summary>
    public enum HouseHoldTypes
    {
        /// <summary>
        /// 其他
        /// </summary>
        [EnumItemDescription("zh-CN", "其他")]
        Other = 0,

        /// <summary>
        /// 本市城镇
        /// </summary>
        [EnumItemDescription("zh-CN", "本市城镇")]
        LocalCity = 1,

        /// <summary>
        /// 本市农村
        /// </summary>
        [EnumItemDescription("zh-CN", "本市农村")]
        LocalCountry = 2,


        /// <summary>
        /// 本省城镇
        /// </summary>
        [EnumItemDescription("zh-CN", "本省城镇")]
        ProvinceCity = 11,

        /// <summary>
        /// 本省农村
        /// </summary>
        [EnumItemDescription("zh-CN", "本省农村")]
        ProvinceCountry = 12,

        /// <summary>
        /// 外省城镇
        /// </summary>
        [EnumItemDescription("zh-CN", "外省城镇")]
        OuterCity = 21,

        /// <summary>
        /// 外省农村
        /// </summary>
        [EnumItemDescription("zh-CN", "外省农村")]
        OuterCountry = 22,
    }
}
