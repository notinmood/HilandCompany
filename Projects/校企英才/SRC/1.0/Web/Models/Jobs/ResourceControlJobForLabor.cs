using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using Quartz;
using XQYC.Business.BLL;
using XQYC.Business.Enums;

namespace XQYC.Web.Models.Jobs
{
    public class ResourceControlJobForLabor : ResourceControlJob
    {
        protected override void ExecuteDetails(JobExecutionContext context)
        {
            int fromCreatedToSignedProtectedDays = 3;
            int fromUpdatedToSignedProtectedDays = 1;
            int fromContractEndToResignedProtectedDays = 7;
            if (SystemTaskInConfig != null)
            {
                //对得到的时间，做一天的修正处理
                fromCreatedToSignedProtectedDays = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("fromCreatedToSignedProtectedDays"), 3) + 1;
                fromUpdatedToSignedProtectedDays = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("fromUpdatedToSignedProtectedDays"), 1) + 1;
                fromContractEndToResignedProtectedDays = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("fromContractEndToResignedProtectedDays"), 7) + 1;
            }

            //1. 释放录入及其修改后在一段时间内都未能形成派遣的劳务人员的保护
            //string sqlClauseForSignProtected = string.Format("update [XQYCLabor] set IsProtectedByOwner ={0} where LaborWorkStatus={1} AND CreateDate< '{2}' AND LastUpdateDate<'{3}' ",
            //    (int)Logics.False, (int)LaborWorkStatuses.NewWorker, DateTime.Today.AddDays(-fromCreatedToSignedProtectedDays), DateTime.Today.AddDays(-fromUpdatedToSignedProtectedDays));

            //int rowCountForCreateUpdateProtected = LaborBLL.Instance.ExcuteNonQuery(sqlClauseForSignProtected);

            //2.释放合同到期之后的一段时间内未能形成二次签约的劳务人员的保护
            string sqlClauseForReSignProtected = string.Format("update [XQYCLabor] set IsProtectedByOwner ={0} where LaborWorkStatus !={1} AND CurrentContractDiscontinueDate<='{2}'  AND CurrentContractDiscontinueDate!='{3}' AND LastUpdateDate<'{4}' ",
                (int)Logics.False, (int)LaborWorkStatuses.Worked, DateTime.Today.AddDays(-fromContractEndToResignedProtectedDays), DateTimeHelper.Min, DateTime.Today.AddDays(fromUpdatedToSignedProtectedDays));

            LaborBLL.Instance.ExcuteNonQuery(sqlClauseForReSignProtected);
            //3.释放****对劳务人员的保护
        }

        protected override string TaskNameInConfig
        {
            get { return "ResourceControlJobForLabor"; }
        }
    }
}