using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    /// <summary>
    /// 人员基本工资项目部门项目分摊
    /// </summary>
    [Serializable()]
    [ORTableMapping("PERSON_BASE_FEE_DEPARTMENT_PROJECT")]
    public class PersonBaseFeeDepartmentProjectInfo
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        [ORFieldMapping("PERSON_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonID { set; get; }

        /// <summary>
        /// 工资项目ID
        /// </summary>
        [ORFieldMapping("FEE_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeID { set; get; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [ORFieldMapping("DEPARTMENT_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String DepartmentID { set; get; }

        /// <summary>
        /// 项目ID
        /// </summary>
        [ORFieldMapping("PROJECT_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectID { set; get; }

        /// <summary>
        /// 姓名
        /// </summary>
        [ORFieldMapping("PERSON_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonName { set; get; }

        /// <summary>
        /// 工资项目名称
        /// </summary>
        [ORFieldMapping("FEE_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeName { set; get; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [ORFieldMapping("DEPARTMENT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String DepartmentName { set; get; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [ORFieldMapping("PROJECT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectName { set; get; }

        /// <summary>
        /// 负担人员岗位工资金额
        /// </summary>
        [ORFieldMapping("STATION_MONEY")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal StationMoney { set; get; }
    }

    [Serializable()]
    public class PersonBaseFeeDepartmentProjectInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PERSON_BASE_FEE_DEPARTMENT_PROJECT";
        /// <summary>
        /// 工资项目ID
        /// </summary>
        public const String FeeID = "FEE_ID";
        /// <summary>
        /// 部门ID
        /// </summary>
        public const String DepartmentID = "DEPARTMENT_ID";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const String ProjectID = "PROJECT_ID";
        /// <summary>
        /// <summary>
        /// 人员ID
        /// </summary>
        public const String PersonID = "PERSON_ID";

    }

    [Serializable()]
    public class PersonBaseFeeDepartmentProjectInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PERSON_BASE_FEE_DEPARTMENT_PROJECT";
        /// <summary>
        /// 工资项目ID
        /// </summary>
        public const String FeeID = "FeeID";
        /// <summary>
        /// 部门ID
        /// </summary>
        public const String DepartmentID = "DepartmentID";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const String ProjectID = "ProjectID";
        /// <summary>
        /// 人员ID
        /// </summary>
        public const String PersonID = "PersonID";
        /// <summary>
        /// 负担人员岗位工资金额
        /// </summary>
        public const String StationMoney = "StationMoney";
    }
}
