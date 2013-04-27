using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;

namespace Salary.Biz
{
    /// <summary>
    /// 月税
    /// </summary>
    [Serializable()]
    [ORTableMapping("TAX_MONTH")]
    public class TaxMonthInfo : TaxInfo
    {
        //private TaxMonthInfo _taxMonthInfo = new TaxMonthInfo();
        public TaxMonthInfo(){}
        public TaxMonthInfo(String yearMonth)
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
    public class TaxMonthInfoDBConst : TaxInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "TAX_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YEAR_MONTH";
    }

    [Serializable()]
    public class TaxMonthInfoConst : TaxInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "TAX_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YearMonth";
    }
}
