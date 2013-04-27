using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;

namespace Salary.Biz
{
    /// <summary>
    /// 部门
    /// </summary>
    [Serializable()]
    [ORTableMapping("DEPARTMENT")]
    public class DepartmentInfo
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        [ORFieldMapping("DEPARTMENT_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String DepartmentID { set; get; }

        /// <summary>
        /// 部门编码
        /// </summary>
        [ORFieldMapping("DEPARTMENT_CODE", IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String DepartmentCode { set; get; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        [ORFieldMapping("PARENT_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ParentID { set; get; }

        /// <summary>
        /// 上级部门名称
        /// </summary>
        [ORFieldMapping("PARENT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ParentName { set; get; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [ORFieldMapping("DEPARTMENT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String DepartmentName { set; get; }

        /// <summary>
        /// 停用标记
        /// </summary>
        [ORFieldMapping("USE_FLAG")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public bool UseFlag { set; get; }
    }


    [Serializable()]
    public class DepartmentInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "DEPARTMENT";
        /// <summary>
        /// 部门ID
        /// </summary>
        public const String DepartmentID = "DEPARTMENT_ID";
        /// <summary>
        /// 部门编码
        /// </summary>
        public const String DepartmentCode = "DEPARTMENT_CODE";
        /// <summary>
        /// 上级部门ID
        /// </summary>
        public const String ParentID = "PARENT_ID";
        /// <summary>
        /// 部门名称
        /// </summary>
        public const String DepartmentName = "DEPARTMENT_NAME";
        /// <summary>
        /// 使用标识
        /// </summary>
        public const string UseFlag = "USE_FLAG";
    }


    [Serializable()]
    public class DepartmentInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "DEPARTMENT";
        /// <summary>
        /// 部门ID
        /// </summary>
        public const String DepartmentID = "DepartmentID";
        /// <summary>
        /// 部门编码
        /// </summary>
        public const String DepartmentCode = "DepartmentCode";
        /// <summary>
        /// 上级部门ID
        /// </summary>
        public const String ParentID = "ParentID";
        /// <summary>
        /// 部门名称
        /// </summary>
        public const String DepartmentName = "DepartmentName";
        /// <summary>
        /// 使用标识
        /// </summary>
        public const string UseFlag = "UseFlag";
    }

}
