using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Enums;
using Quartz;

namespace XQYC.Web.Models.Jobs
{
    /// <summary>
    /// 劳务人员合同到期提醒
    /// </summary>
    public class LaborContractRemindJob : RemindJob
    {
        protected override void ExecuteDetails(JobExecutionContext context)
        {
            //
        }

        protected override string TaskNameInConfig
        {
            get { return "LaborContractRemindTask"; }
        }
    }
}