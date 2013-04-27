using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Salary.Core.Utility
{
    public static class StrHelper
    {
        public static string FillUpDefaultString(int value, int length, string defaultString, bool isLeft)
        {
            string result = value.ToString();
            if (result.Length < length)
            {
                string s = Enumerable.Repeat(defaultString, length - result.Length).Aggregate((pred, next) => pred + next);
                result = isLeft ? (s + result) : (result + s);
            }
            return result;
        }
        /// <summary>
        /// 判断字符串是否为Int类型的
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsValidInt(string val)
        {
            return Regex.IsMatch(val, @"^[0-9]\d*\.?[0]*$");
        }

        public static bool IsValidDecimal(string val)
        {
            return Regex.IsMatch(val, @"^(\d{1,18})(\.\d{1,3})?$");
        }

        /// <summary>
        /// 前补0
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string PreFillZero(int num, int length)
        {
            string newNum = num.ToString();
            string snum = num.ToString();
            int numlen = snum.Length;
            for (int i = 0; i < (length - numlen); i++)
            {
                newNum = "0" + newNum;
            }
            return newNum;
        }
    }
}
