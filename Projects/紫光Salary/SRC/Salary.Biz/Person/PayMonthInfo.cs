using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;

namespace Salary.Biz
{
    /// <summary>
    /// 人员部门(月)
    /// </summary>
    [Serializable()]
    [ORTableMapping("PAY_MONTH")]
    public class PayMonthInfo : PersonBaseFeeInfo
    {
        public PayMonthInfo(){}
        public PayMonthInfo(String yearMonth)
        {
            YearMonth = yearMonth;
        }

        /// <summary>
        /// 年月
        /// </summary>
        [ORFieldMapping("YEAR_MONTH", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public string YearMonth { set; get; }

        /// <summary>
        /// 款项金额
        /// </summary>
        [ORFieldMapping("PAY_MONEY")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal PayMoney { set; get; }

        /// <summary>
        /// 计算公式
        /// </summary>
        [ORFieldMapping("FORMULA")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String Formula { set; get; }
    }

    [Serializable()]
    public class PayMonthInfoDBConst : PersonBaseFeeInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PAY_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YEAR_MONTH";
    }

    [Serializable()]
    public class PayMonthInfoConst : PersonBaseFeeInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "PAY_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YearMonth";
    }
}
