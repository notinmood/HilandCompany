using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XQYC.Web.Control
{
    /// <summary>
    /// 
    /// </summary>
    public enum XQYCTreeNodeSelectModes
    {
        /// <summary>
        /// 允许选择部门
        /// </summary>
        Department=1,
        /// <summary>
        /// 允许选择人员
        /// </summary>
        Employee=2,
        /// <summary>
        /// 部门和人员都可以选择
        /// </summary>
        All=3,
    }
}