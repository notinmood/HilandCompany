using System;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Mapping;
using System.Collections.Generic;
using ChinaCustoms.Framework.DeluxeWorks.Library.Data.Builder;

namespace Salary.Biz
{
    /// <summary>
    /// 税
    /// </summary>
    [Serializable()]
    [ORTableMapping("TAX")]
    public class TaxInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        [ORFieldMapping("TAX_ID", PrimaryKey = true, IsNullable = false)]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String TaxID { set; get; }

        /// <summary>
        /// 款项ID
        /// </summary>
        [ORFieldMapping("FEE_ID")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeID { set; get; }

        /// <summary>
        /// 款项名称
        /// </summary>
        [ORFieldMapping("FEE_NAME")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public String FeeName { set; get; }

        /// <summary>
        /// 应付金额大于
        /// </summary>
        [ORFieldMapping("QUANTUM_START")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal QuantumStart { set; get; }

        /// <summary>
        /// 应付金额小于
        /// </summary>
        [ORFieldMapping("QUANTUM_END")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal QuantumEnd { set; get; }

        /// <summary>
        /// 税率
        /// </summary>
        [ORFieldMapping("RATE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal Rate { set; get; }

        /// <summary>
        /// 速算减扣金额
        /// </summary>
        [ORFieldMapping("SUBTRACT")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal Subtract { set; get; }

        /// <summary>
        /// 减扣系数
        /// </summary>
        [ORFieldMapping("SUBTRACT_MULTIPLE")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal SubtractMultiple { set; get; }

        /// <summary>
        /// 减扣金额
        /// </summary>
        [ORFieldMapping("SUBTRACT_MONEY")]
        [SqlBehavior(BindingFlags = ClauseBindingFlags.All)]
        public decimal SubtractMoney { set; get; }
    }


    [Serializable()]
    public class TaxInfoDBConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "TAX";
        /// <summary>
        /// TaxID
        /// </summary>
        public const String TaxID = "TAX_ID";
        /// <summary>
        /// 款项ID
        /// </summary>
        public const String FeeID = "FEE_ID";
        /// <summary>
        /// 款项名称
        /// </summary>
        public const String FeeName = "FEE_NAME";
        /// <summary>
        /// 应付金额大于
        /// </summary>
        public const String QuantumStart = "QUANTUM_START";
        /// <summary>
        /// 应付金额小于
        /// </summary>
        public const String QuantumEnd = "QUANTUM_END";
    }

    [Serializable()]
    public class TaxInfoConst
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "TAX";
        /// <summary>
        /// TaxID
        /// </summary>
        public const String TaxID = "TaxID";
        /// <summary>
        /// 款项ID
        /// </summary>
        public const String FeeID = "FeeID";
        /// <summary>
        /// 款项名称
        /// </summary>
        public const String FeeName = "FeeName";
        /// <summary>
        /// 应付金额大于
        /// </summary>
        public const String QuantumStart = "QuantumStart";
        /// <summary>
        /// 应付金额小于
        /// </summary>
        public const String QuantumEnd = "QuantumEnd";
    }
}
