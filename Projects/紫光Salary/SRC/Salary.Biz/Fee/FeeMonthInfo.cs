using System;
using System.Linq;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    /// <summary>
    /// 月工资项目
    /// </summary>
    [Serializable()]
    [ORTableMapping("FEE_MONTH")]
    public class FeeMonthInfo : FeeInfo
    {
        //private FeeMonthInfo _feeMonthInfo = new FeeMonthInfo();
        public FeeMonthInfo(){}
        public FeeMonthInfo(String yearMonth)
        {
            YearMonth = yearMonth;
        }

        /// <summary>
        /// 年月
        /// </summary>
        [ORFieldMapping("YEAR_MONTH", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String YearMonth { set; get; }
    }

    [Serializable()]
    public class FeeMonthInfoDBConst : FeeInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "FEE_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YEAR_MONTH";
    }

    [Serializable()]
    public class FeeMonthInfoConst : FeeInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "FEE_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YearMonth";
    }
}
