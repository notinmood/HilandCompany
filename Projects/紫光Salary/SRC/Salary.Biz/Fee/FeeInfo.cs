using System;
using System.Linq;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;
using Salary.Biz.Eunm;

namespace Salary.Biz
{
    /// <summary>
    /// 工资项目
    /// </summary>
    [Serializable()]
    [ORTableMapping("FEE")]
    public class FeeInfo
    {
        #region 基本属性

        /// <summary>
        /// 款项ID
        /// </summary>
        [ORFieldMapping("FEE_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeID { set; get; }

        /// <summary>
        /// 项目编码
        /// </summary>
        [ORFieldMapping("FEE_CODE", IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeCode { set; get; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [ORFieldMapping("FEE_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeName { set; get; }

        /// <summary>
        /// 父项ID
        /// </summary>
        [ORFieldMapping("PARENT_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ParentID { set; get; }

        /// <summary>
        /// 父项名称
        /// </summary>
        [ORFieldMapping("PARENT_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String ParentName { set; get; }

        /// <summary>
        /// 运算符号
        /// </summary>
        [ORFieldMapping("CALCULATE_SIGN")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String CalculateSign { set; get; }

        /// <summary>
        /// 款项类型
        /// </summary>
        [ORFieldMapping("FEE_TYPE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All, EnumUsage = EnumUsageTypes.UseEnumValue)]
        public FeeType FeeType { set; get; }

        /// <summary>
        /// 基本款项类型
        /// </summary>
        [ORFieldMapping("COMMON_FEE_TYPE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All, EnumUsage = EnumUsageTypes.UseEnumValue)]
        public CommonFeeType CommonFeeType { set; get; }

        /// <summary>
        /// 初始值
        /// </summary>
        [ORFieldMapping("DEFAULT_VALUE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public Decimal DefaultValue { set; get; }

        /// <summary>
        /// 计算公式
        /// </summary>
        [ORFieldMapping("CALCULATE_EXP")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String CalculateExp { set; get; }

        /// <summary>
        /// 应税项目
        /// </summary>
        [ORFieldMapping("TAX_TARGET_FEE_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String TaxTargetFeeID { set; get; }

        /// <summary>
        /// 个税起征额
        /// </summary>
        [ORFieldMapping("TAX_BASE_VALUE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public Decimal TaxBaseValue { set; get; }

        /// <summary>
        /// 停用标记
        /// </summary>
        [ORFieldMapping("USE_FLAG")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public bool UseFlag { set; get; }

        /// <summary>
        /// 启用日期
        /// </summary>
        [ORFieldMapping("START_DATE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public DateTime StartDate { set; get; }

        #endregion 基本属性

        #region 扩展属性

        [NoMapping]
        public List<TaxInfo> TaxInfoList
        {
            get
            {
                if (_TaxInfoList == null)
                {
                    WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
                    builder.AppendItem(TaxInfoDBConst.FeeID, FeeID);
                    _TaxInfoList = TaxInfoAdapter.Instance.GetTaxInfoList(null, builder);
                }
                return _TaxInfoList;
            }
        }
        private List<TaxInfo> _TaxInfoList = null;

        #endregion 扩展属性
    }


    [Serializable()]
    public class FeeInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "FEE";
        /// <summary>
        /// 款项ID
        /// </summary>
        public const String FeeID = "FEE_ID";
        /// <summary>
        /// 款项编码
        /// </summary>
        public const String FeeCode = "FEE_CODE";
        /// <summary>
        /// 款项名称
        /// </summary>
        public const String FeeName = "FEE_NAME";
        /// <summary>
        /// 父项ID
        /// </summary>
        public const String ParentID = "PARENT_ID";
        /// <summary>
        /// 父项名称
        /// </summary>
        public const String ParentName = "PARENT_NAME";
        /// <summary>
        /// 运算符号
        /// </summary>
        public const String CalculateSign = "CALCULATE_SIGN";
        /// <summary>
        /// 款项类型
        /// </summary>
        public const String FeeType = "FEE_TYPE";
        /// <summary>
        /// 基本款项类型
        /// </summary>
        public const String CommonFeeType = "COMMON_FEE_TYPE";
        /// <summary>
        /// 初始值
        /// </summary>
        public const String DefaultValue = "DEFAULT_VALUE";
        /// <summary>
        /// 工资项目计算公式
        /// </summary>
        public const String CalculateExp = "CALCULATE_EXP";
        /// <summary>
        /// 使用标识
        /// </summary>
        public const String UseFlag = "USE_FLAG";
        /// <summary>
        /// 启用日期
        /// </summary>
        public const String StartDate = "START_DATE";
    }

    [Serializable()]
    public class FeeInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "FEE";
        /// <summary>
        /// 款项ID
        /// </summary>
        public const String FeeID = "FeeID";
        /// <summary>
        /// 款项编码
        /// </summary>
        public const String FeeCode = "FeeCode";
        /// <summary>
        /// 款项名称
        /// </summary>
        public const String FeeName = "FeeName";
        /// <summary>
        /// 父项ID
        /// </summary>
        public const String ParentID = "ParentID";
        /// <summary>
        /// 父项名称
        /// </summary>
        public const String ParentName = "ParentName";
        /// <summary>
        /// 运算符号
        /// </summary>
        public const String CalculateSign = "CalculateSign ";
        /// <summary>
        /// 款项类型
        /// </summary>
        public const String FeeType = "FeeType";
        /// <summary>
        /// 基本款项类型
        /// </summary>
        public const String CommonFeeType = "CommonFeeType";
        /// <summary>
        /// 使用标识
        /// </summary>
        public const String UseFlag = "UseFlag";
        /// <summary>
        /// 启用日期
        /// </summary>
        public const String StartDate = "StartDate";
    }
}