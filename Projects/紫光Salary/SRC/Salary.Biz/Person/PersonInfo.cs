using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    /// <summary>
    /// PersonInfo
    /// </summary>
    [Serializable()]
    [ORTableMapping("PERSON")]
    public class PersonInfo
    {
        #region 基本属性

        /// <summary>
        /// 人员ID
        /// </summary>
        [ORFieldMapping("PERSON_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonID { set; get; }

        /// <summary>
        /// 父ID
        /// </summary>
        [ORFieldMapping("PARENT_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ParentID { set; get; }

        /// <summary>
        /// 人员编码
        /// </summary>
        [ORFieldMapping("PERSON_CODE", IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonCode { set; get; }

        /// <summary>
        /// 姓名
        /// </summary>
        [ORFieldMapping("PERSON_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonName { set; get; }

        /// <summary>
        /// 性别
        /// </summary>
        [ORFieldMapping("GENDER")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String Gender { set; get; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [ORFieldMapping("DEPARTMENT_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String DepartmentID { set; get; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [ORFieldMapping("DEPARTMENT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String DepartmentName { set; get; }

        /// <summary>
        /// 项目ID
        /// </summary>
        [ORFieldMapping("PROJECT_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectID { set; get; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [ORFieldMapping("PROJECT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectName { set; get; }

        /// <summary>
        /// 入职日期
        /// </summary>
        [ORFieldMapping("ENTRY_DATE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public DateTime EntryDate { set; get; }

        /// <summary>
        /// 离职日期
        /// </summary>
        [ORFieldMapping("LEFT_DATE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public DateTime LeftDate { set; get; }

        /// <summary>
        /// 离职标记
        /// </summary>
        [ORFieldMapping("DIMISSION")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public bool Dimission { set; get; }

        /// <summary>
        /// 人员类型
        /// </summary>
        [ORFieldMapping("PERSON_TYPE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All, EnumUsage = EnumUsageTypes.UseEnumValue)]
        public PersonType PersonType { set; get; }

        #endregion 基本属性

        #region 扩展属性

        //[NoMapping]
        //public List<PersonBaseFeeInfo> PersonBaseFeeInfoList
        //{
        //    get
        //    {
        //        if (_PersonBaseFeeInfoList == null)
        //        {
        //            _PersonBaseFeeInfoList = PersonBaseFeeInfoAdapter.Instance.GetPersonBaseFeeInfoList(null);
        //        }
        //        return _PersonBaseFeeInfoList;
        //    }
        //}
        //private List<PersonBaseFeeInfo> _PersonBaseFeeInfoList = null;

        #endregion 扩展属性
    }
    
    [Serializable()]
    public class PersonInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PERSON";
        /// <summary>
        /// 人员ID
        /// </summary>
        public const String PersonID = "PERSON_ID";
        /// <summary>
        /// 父ID
        /// </summary>
        public const String ParentID = "PARENT_ID";
        /// <summary>
        /// 人员编码
        /// </summary>
        public const String PersonCode = "PERSON_CODE";
        /// <summary>
        /// 姓名
        /// </summary>
        public const String PersonName = "PERSON_NAME";
        /// <summary>
        /// 性别
        /// </summary>
        public const String Gender = "GENDER";
        /// <summary>
        /// 人员类型
        /// </summary>
        public const String PersonType = "PERSON_TYPE";
        /// <summary>
        /// 部门ID
        /// </summary>
        public const String DepartmentID = "DEPARTMENT_ID";
        /// <summary>
        /// 部门名称
        /// </summary>
        public const String DepartmentName = "DEPARTMENT_NAME";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const String ProjectID = "PROJECT_ID";
        /// <summary>
        /// 项目名称
        /// </summary>
        public const String ProjectName = "PROJECT_NAME";
        /// <summary>
        /// 入职日期
        /// </summary>
        public const String EntryDate = "ENTRY_DATE";
        /// <summary>
        /// 离职日期
        /// </summary>
        public const String LeftDate = "LEFT_DATE";
        /// <summary>
        /// 离职标识
        /// </summary>
        public const string Dimission = "DIMISSION";
    }

    [Serializable()]
    public class PersonInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PERSON";
        /// <summary>
        /// 人员ID
        /// </summary>
        public const String PersonID = "PersonID";
        /// <summary>
        /// 父ID
        /// </summary>
        public const String ParentID = "ParentID";
        /// <summary>
        /// 姓名
        /// </summary>
        public const String PersonName = "PersonName";
        /// <summary>
        /// 性别
        /// </summary>
        public const String Gender = "Gender";
        /// <summary>
        /// 部门ID
        /// </summary>
        public const string DepartmentID = "DepartmentID";
        /// <summary>
        /// 人员类型
        /// </summary>
        public const string PersonType = "PersonType";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const string ProjectID = "ProjectID";
        /// <summary>
        /// 入职日期
        /// </summary>
        public const String EntryDate = "EntryDate";
        /// <summary>
        /// 离职日期
        /// </summary>
        public const String LeftDate = "LeftDate";
        /// <summary>
        /// 离职标识
        /// </summary>
        public const string Dimission = "Dimission";
    }
}
