using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Utility.Enums.OP;

namespace XQYC.Business.Enums
{
    /// <summary>
    /// 劳务人员的工作状态
    /// </summary>
    public enum LaborWorkStatuses
    {
        /// <summary>
        /// 合同异常终止（开除等企业因素）
        /// </summary>
        [EnumItemDescription("zh-CN", "企业终止合同")]
        UnNormalEnterpriseStop = -20,

        /// <summary>
        /// 合同异常终止（辞职等个人因素）
        /// </summary>
        [EnumItemDescription("zh-CN", "个人终止合同")]
        UnNormalPersenalStop = -10,

        /// <summary>
        /// 新人
        /// </summary>
        [EnumItemDescription("zh-CN", "新人")]
        NewWorker = 0,

        /// <summary>
        /// 合同到期等自然终止
        /// </summary>
        [EnumItemDescription("zh-CN", "合同到期终止")]
        NormalStop = 10,

        /// <summary>
        /// 在职中
        /// </summary>
        [EnumItemDescription("zh-CN", "在职中")]
        Worked = 20,
    }
}
