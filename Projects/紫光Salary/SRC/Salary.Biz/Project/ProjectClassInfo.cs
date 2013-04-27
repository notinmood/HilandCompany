using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;

namespace Salary.Biz
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    [ORTableMapping("PROJECT_CLASS")]
    public class ProjectClassInfo
    {
        /// <summary>
        /// 项目分类ID
        /// </summary>
        [ORFieldMapping("PROJECT_CLASS_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectClassID { set; get; }

        /// <summary>
        /// 上级项目分类ID
        /// </summary>
        [ORFieldMapping("PARENT_CLASS_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ParentClassID { set; get; }

        /// <summary>
        /// 上级项目分类名称
        /// </summary>
        [ORFieldMapping("PARENT_CLASS_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ParentClassName { set; get; }

        /// <summary>
        /// 项目分类名称
        /// </summary>
        [ORFieldMapping("PROJECT_CLASS_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectClassName { set; get; }

        /// <summary>
        /// 停用标记
        /// </summary>
        [ORFieldMapping("USE_FLAG")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public bool UseFlag { set; get; }
    }

    [Serializable()]
    public class ProjectClassInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PROJECT_CLASS";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const String ProjectClassID = "PROJECT_CLASS_ID";
        /// <summary>
        /// 项目名称
        /// </summary>
        public const String ProjectClassName = "PROJECT_CLASS_NAME";
        /// <summary>
        /// 上级项目ID
        /// </summary>
        public const String ParentClassID = "PARENT_CLASS_ID";
        /// <summary>
        /// 停用标记
        /// </summary>
        public const String UseFlag = "USE_FLAG";

    }

    [Serializable()]
    public class ProjectClassInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "ProjectClass";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const String ProjectClassID = "ProjectClassID";
        /// <summary>
        /// 项目名称
        /// </summary>
        public const String ProjectClassName = "ProjectClassName";
        /// <summary>
        /// 上级项目ID
        /// </summary>
        public const String ParentClassID = "ParentClassID";
        /// <summary>
        /// 停用标记
        /// </summary>
        public const String UseFlag = "UseFlag";
    }
}
