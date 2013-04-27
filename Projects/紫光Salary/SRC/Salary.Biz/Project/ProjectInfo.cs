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
    [ORTableMapping("PROJECT")]
    public class ProjectInfo
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [ORFieldMapping("PROJECT_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectId { set; get; }

        /// <summary>
        /// 项目编码
        /// </summary>
        [ORFieldMapping("PROJECT_CODE", IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectCode { set; get; }

        /// <summary>
        /// 项目分类ID
        /// </summary>
        [ORFieldMapping("PROJECT_CLASS_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectClassID { set; get; }

        /// <summary>
        /// 项目分类名称
        /// </summary>
        [ORFieldMapping("PROJECT_CLASS_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectClassName { set; get; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [ORFieldMapping("PROJECT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ProjectName { set; get; }

        /// <summary>
        /// 停用标记
        /// </summary>
        [ORFieldMapping("USE_FLAG")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public bool UseFlag { set; get; }
    }


    [Serializable()]
    public class ProjectInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PROJECT";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const String ProjectID = "PROJECT_ID";
        /// <summary>
        /// 项目编码
        /// </summary>
        public const String ProjectCode = "PROJECT_CODE";
        /// <summary>
        /// 项目名称
        /// </summary>
        public const String ProjectName = "PROJECT_NAME";
        /// <summary>
        /// 项目分类ID
        /// </summary>
        public const String ProjectClassID = "PROJECT_CLASS_ID";
        /// <summary>
        /// 停用标记
        /// </summary>
        public const String UseFlag = "USE_FLAG";

    }

    [Serializable()]
    public class ProjectInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "PROJECT";
        /// <summary>
        /// 项目ID
        /// </summary>
        public const String ProjectID = "ProjectID";
        /// <summary>
        /// 项目名称
        /// </summary>
        public const String ProjectName = "ProjectName";
        /// <summary>
        /// 项目分类ID
        /// </summary>
        public const String ProjectClassID = "ProjectClassID";
        /// <summary>
        /// 停用标记
        /// </summary>
        public const String UseFlag = "UseFlag";
    }
}
