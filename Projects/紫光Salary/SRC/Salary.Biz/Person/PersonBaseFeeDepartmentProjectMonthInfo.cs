using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;

namespace Salary.Biz
{
    /// <summary>
    /// 人员部门项目(月)
    /// </summary>
    [Serializable()]
    [ORTableMapping("PERSON_FEE_DEPARTMENT_PROJECT_MONTH")]
    public class PersonBaseFeeDepartmentProjectMonthInfo : PersonBaseFeeDepartmentProjectInfo
    {
        //private PersonBaseFeeDepartmentProjectMonthInfo _personBaseFeeDepartmentProjectMonthInfo = new PersonBaseFeeDepartmentProjectMonthInfo();
        public PersonBaseFeeDepartmentProjectMonthInfo(){}
        public PersonBaseFeeDepartmentProjectMonthInfo(String yearMonth)
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
    public class PersonBaseFeeDepartmentProjectMonthInfoDBConst : PersonBaseFeeDepartmentProjectInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "PERSON_FEE_DEPARTMENT_PROJECT_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YEAR_MONTH";
    }

    [Serializable()]
    public class PersonBaseFeeDepartmentProjectMonthInfoConst : PersonBaseFeeDepartmentProjectInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected new const String TableName = "PERSON_FEE_DEPARTMENT_PROJECT_MONTH";
        /// <summary>
        /// 年月
        /// </summary>
        public const String YearMonth = "YearMonth";
    }
}
