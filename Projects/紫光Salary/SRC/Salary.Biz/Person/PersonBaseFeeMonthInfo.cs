using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    /// <summary>
    /// 人员月基本款项
    /// </summary>
    [Serializable()]
    [ORTableMapping("PERSON_BASE_FEE_MONTH")]
    public class PersonBaseFeeMonthInfo : PersonBaseFeeInfo
    {
        //private PersonBaseFeeMonthInfo _personBaseFeeMonthInfo = new PersonBaseFeeMonthInfo();
        public PersonBaseFeeMonthInfo(){}
        public PersonBaseFeeMonthInfo(String yearMonth)
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
    public class PersonBaseFeeMonthInfoDBConst : PersonBaseFeeInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "PERSON_BASE_FEE_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YEAR_MONTH";
    }

    [Serializable()]
    public class PersonBaseFeeMonthInfoConst : PersonBaseFeeInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "PERSON_BASE_FEE_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YearMonth";
    }
}
