using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Biz.Eunm;

namespace Salary.Biz
{    
    /// <summary>
    /// 人员基本款项
    /// </summary>
    [Serializable()]
    [ORTableMapping("PERSON_BASE_FEE")]
    public class PersonBaseFeeInfo
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        [ORFieldMapping("PERSON_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonID { set; get; }

        /// <summary>
        /// 姓名
        /// </summary>
        [ORFieldMapping("PERSON_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonName { set; get; }

        /// <summary>
        /// 款项ID
        /// </summary>
        [ORFieldMapping("FEE_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeID { set; get; }

        /// <summary>
        /// 款项编码
        /// </summary>
        [ORFieldMapping("FEE_CODE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.Select)]
        public String FeeCode { set; get; }

        /// <summary>
        /// 款项名称
        /// </summary>
        [ORFieldMapping("FEE_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeName { set; get; }

        /// <summary>
        /// 款项值
        /// </summary>
        [ORFieldMapping("FEE_VALUE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal FeeValue { set; get; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [ORFieldMapping("DEPARTMENT_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String DepartmentID { set; get; }

        /// <summary>
        /// 项目ID
        /// </summary>
        [ORFieldMapping("PROJECT_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectID { set; get; }

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
        /// 计提/待摊
        /// </summary>
        [ORFieldMapping("JITI_DAITAN")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public int JitiDaitan { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        [ORFieldMapping("MEMO")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public string Memo { set; get; }
    }

    [Serializable()]
    public class PersonBaseFeeInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PERSON_BASE_FEE";
        /// <summary>
        /// 人员ID
        /// </summary>
        public const String PersonID = "PERSON_ID";
        /// <summary>
        /// 款项编号
        /// </summary>
        public const String FeeID = "FEE_ID";
        /// <summary>
        /// 款项名称
        /// </summary>
        public const String FeeName = "FEE_NAME";
    }

    [Serializable()]
    public class PersonBaseFeeInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PERSON_BASE_FEE";
        /// <summary>
        /// 人员ID
        /// </summary>
        public const String PersonID = "PersonID";
        /// <summary>
        /// 款项编号
        /// </summary>
        public const String FeeID = "FeeID";
        /// <summary>
        /// 款项名称
        /// </summary>
        public const String FeeName = "FeeName";
        /// <summary>
        /// 部门ID
        /// </summary>
        public const String DepartmentID = "DepartmentID";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const String ProjectID = "ProjectID";

        /// <summary>
        /// 款项值
        /// </summary>
        public const String FeeValue = "FeeValue";
    }
}
