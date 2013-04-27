using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    /// <summary>
    /// 报表
    /// </summary>
    [Serializable()]
    [ORTableMapping("REPORT")]
    public class ReportInfo
    {
        /// <summary>
        /// 报表ID
        /// </summary>
        [ORFieldMapping("REPORT_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ReportID { set; get; }

        /// <summary>
        /// 报表编码
        /// </summary>
        [ORFieldMapping("REPORT_CODE", IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ReportCode { set; get; }

        /// <summary>
        /// 报表名称
        /// </summary>
        [ORFieldMapping("REPORT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ReportName { set; get; }

        /// <summary>
        /// 停用标记
        /// </summary>
        [ORFieldMapping("USE_FLAG")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public bool UseFlag { set; get; }
    }

    [Serializable()]
    public class ReportInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "REPORT";
        /// <summary>
        /// 报表ID
        /// </summary>
        public const String ReportID = "REPORT_ID";
        /// <summary>
        /// 报表编码
        /// </summary>
        public const String ReportCode = "REPORT_CODE";
        /// <summary>
        /// 报表名称
        /// </summary>
        public const String ReportName = "REPORT_NAME";
        /// <summary>
        /// 停用标记
        /// </summary>
        public const String UserFlag = "USE_FLAG";
    }

    [Serializable()]
    public class ReportInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "REPORT";
        /// <summary>
        /// 报表ID
        /// </summary>
        public const String ReportID = "ReportID";
        /// <summary>
        /// 报表编码
        /// </summary>
        public const String ReportCode = "ReportCode";
        /// <summary>
        /// 报表名称
        /// </summary>
        public const String ReportName = "ReportName";
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    [ORTableMapping("REPORT_FEE")]
    public class ReportFee
    {
        /// <summary>
        /// 报表ID
        /// </summary>
        [ORFieldMapping("REPORT_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ReportID { set; get; }

        /// <summary>
        /// 款项ID
        /// </summary>
        [ORFieldMapping("FEE_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeID { set; get; }

        /// <summary>
        /// 款项名称
        /// </summary>
        [ORFieldMapping("FEE_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeName { set; get; }

        /// <summary>
        /// 款项别名
        /// </summary>
        [ORFieldMapping("REPORT_FEE_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ReportFeeName { set; get; }

        /// <summary>
        /// 排序号
        /// </summary>
        [ORFieldMapping("ORDER_NO")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public int OrderNo { set; get; }

        /// <summary>
        /// 款项类型
        /// </summary>
        [ORFieldMapping("FEE_TYPE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.Select, EnumUsage = EnumUsageTypes.UseEnumValue)]
        public FeeType FeeType { set; get; }

        /// <summary>
        /// 选中
        /// </summary>
        [ORFieldMapping("CHECKED")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.Select)]
        public Boolean Checked { set; get; }
    }

    [Serializable()]
    public class ReportFeeDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "REPORT_FEE";
        /// <summary>
        /// 报表ID
        /// </summary>
        public const String ReportID = "REPORT_ID";
        /// <summary>
        /// 款项别名
        /// </summary>
        public const String ReportFeeName = "REPORT_FEE_NAME";
        /// <summary>
        /// 款项ID
        /// </summary>
        public const String FeeID = "FEE_ID";
        /// <summary>
        /// 排序号
        /// </summary>
        public const String OrderNo = "ORDER_NO";
    }

    [Serializable()]
    public class ReportFeeConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "REPORT_FEE";
        /// <summary>
        /// 报表ID
        /// </summary>
        public const String ReportID = "ReportID";
        /// <summary>
        /// 款项别名
        /// </summary>
        public const String ReportFeeName = "ReportFeeName";
        /// <summary>
        /// 款项ID
        /// </summary>
        public const String FeeID = "FeeID";
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    [ORTableMapping("REPORT_PERSON")]
    public class ReportPerson
    {
        /// <summary>
        /// 报表ID
        /// </summary>
        [ORFieldMapping("REPORT_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ReportID { set; get; }

        /// <summary>
        /// 人员ID
        /// </summary>
        [ORFieldMapping("PERSON_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonId { set; get; }

        /// <summary>
        /// 姓名
        /// </summary>
        [ORFieldMapping("PERSON_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String PersonName { set; get; }

        /// <summary>
        /// 排序号
        /// </summary>
        [ORFieldMapping("ORDER_NO")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public int OrderNo { set; get; }
    }
    
    [Serializable()]
    public class ReportPersonDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "REPORT_PERSON";
        /// <summary>
        /// 报表ID
        /// </summary>
        public const String ReportID = "REPORT_ID";
        /// <summary>
        /// 报表名称
        /// </summary>
        public const String ReportName = "REPORT_NAME";
        /// <summary>
        /// 人员ID
        /// </summary>
        public const String PersonID = "PERSON_ID";
    }


}
