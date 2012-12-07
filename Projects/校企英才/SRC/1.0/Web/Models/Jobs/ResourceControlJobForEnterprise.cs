using System;
using HiLand.General.BLL;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using Quartz;

namespace XQYC.Web.Models.Jobs
{
    public class ResourceControlJobForEnterprise : ResourceControlJob
    {
        protected override void ExecuteDetails(JobExecutionContext context)
        {
            int fromCreatedToSignedProtectedDays = 3;
            int fromUpdatedToSignedProtectedDays = 1;
            int fromContractEndToResignedProtectedDays = 7;
            if (SystemTaskInConfig != null)
            {
                //对得到的时间，做一天的修正处理
                fromCreatedToSignedProtectedDays = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("fromCreatedToSignedProtectedDays"), 3) - 1;
                fromUpdatedToSignedProtectedDays = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("fromUpdatedToSignedProtectedDays"), 1) - 1;
                fromContractEndToResignedProtectedDays = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("fromContractEndToResignedProtectedDays"), 7) - 1;
            }

            //1. 释放录入及其修改后在一段时间内都未能形成签约的企业的保护
            string sqlClauseForSignProtected = string.Format("update [GeneralEnterprise] set IsProtectedByOwner ={0} where (CooperateStatus= 0 OR CooperateStatus is null ) AND CreateDate< '{1}' AND ( LastUpdateDate<'{2}' OR LastUpdateDate is null ) ",
                (int)Logics.False, DateTime.Today.AddDays(-fromCreatedToSignedProtectedDays), DateTime.Today.AddDays(-fromUpdatedToSignedProtectedDays));

            int rowCountForCreateUpdateProtected = EnterpriseBLL.Instance.ExcuteNonQuery(sqlClauseForSignProtected);

            //2.释放合同到期之后的一段时间内未能形成二次签约的企业的保护
            //string sqlClauseForReSignProtected = string.Format("update [XQYCLabor] set IsProtectedByOwner ={0} where LaborWorkStatus={1} AND CurrentContractStopDate!='{2}' AND CurrentContractStopDate< '{3}' AND LastUpdateDate<'{4}' ",
            //    (int)Logics.True, (int)LaborWorkStatuses.NewWorker, DateTime.Today.AddDays(-fromCreatedToSignedProtectedDays), DateTime.Today.AddDays(fromUpdatedToSignedProtectedDays));
        }

        protected override string TaskNameInConfig
        {
            get { return "ResourceControlJobForEnterprise"; }
        }
    }
}