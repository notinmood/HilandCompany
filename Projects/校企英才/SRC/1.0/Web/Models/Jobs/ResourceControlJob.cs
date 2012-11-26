using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XQYC.Web.Models.Jobs
{
    /// <summary>
    /// 资源控制类别的系统任务基类
    /// </summary>
    public abstract class ResourceControlJob : SystemJob
    {
        /// <summary>
        /// 日志类别的名称
        /// </summary>
        protected override string LogCategoryName
        {
            get { return "ResourceControl"; }
        }
    }
}