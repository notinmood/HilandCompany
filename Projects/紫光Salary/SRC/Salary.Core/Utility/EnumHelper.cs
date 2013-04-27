using System;
using System.Linq;
using ChinaCustoms.Framework.DeluxeWorks.Library.Core;

namespace Salary.Core.Utility
{
    public static class EnumHelper
    {
        /// <summary>
        /// 根据数字或代码字符串解析为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="EnumValue">数字或代码字符串</param>
        /// <returns>枚举值</returns>
        public static T Parse<T>(String enumValue)
        {
            enumValue = string.IsNullOrEmpty(enumValue) ? "0" : enumValue;
            return (T)Enum.Parse(typeof(T), enumValue);
        }

        public static bool In(this Enum enumValue, params object[] objArray)
        {
            return objArray.Any(v => enumValue.ToString() == v.ToString());
        }

        public static T ParseNull<T>()
        {
            return Parse<T>(String.Empty);
        }

        /// <summary>
        /// 把枚举类型数组转为逗号分隔的数字，如：1,3,34，便于SQL中的IN操作
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="array">数组</param>
        /// <returns>字符串</returns>
        public static string EnumIntToStr(object[] objArray)
        {
            return objArray.Select(i => ((int)i).ToString()).Aggregate((s1, s2) => s1 + "," + s2);
        }

        /// <summary>
        /// 根据数字或代码字符串得到描述
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="EnumValue">数字或代码字符串</param>
        /// <returns>描述</returns>
        public static string GetDescription<T>(string enumValue)
        {
            return GetDescription((System.Enum)Enum.Parse(typeof(T), enumValue));
        }

        public static string GetDescription(Enum enumValue)
        {
            return EnumItemDescriptionAttribute.GetDescription(enumValue);
        }

        public static string Description(this Enum enumValue)
        {
            string description = string.Empty;
            try
            {
                description = EnumItemDescriptionAttribute.GetDescription(enumValue);
            }
            catch
            {
            }
            return description;
        }
    }
}
