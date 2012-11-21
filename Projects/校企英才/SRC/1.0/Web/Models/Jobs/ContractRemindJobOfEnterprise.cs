using System;
using System.Collections.Generic;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.General;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using Quartz;
using XQYC.Business.BLL;
using XQYC.Business.Entity;

namespace XQYC.Web.Models.Jobs
{
    /// <summary>
    /// 企业合同到期提醒
    /// </summary>
    public class ContractRemindJobOfEnterprise : RemindJob
    {
        /// <summary>
        /// 企业合同到期的提醒发给所有的内部人员
        /// </summary>
        /// <param name="context"></param>
        protected override void ExecuteDetails(JobExecutionContext context)
        {
            int daysOffToday = 3;

            if (SystemTaskInConfig != null)
            {
                daysOffToday = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("aheadDays"), 7);
            }

            DateTime dateLower = DateTime.Today.AddDays(daysOffToday);
            DateTime dateUpper = dateLower.AddDays(1);

            List<EnterpriseContractEntity> enterpriseContractList = EnterpriseContractBLL.Instance.GetList(string.Format("[ContractStatus] ={0} AND ContractStopDate>= '{1}'  AND ContractStopDate<'{2}' ", (int)Logics.True, dateLower, dateUpper));
            DispatchRemindMessage(enterpriseContractList);
        }

        private void DispatchRemindMessage(List<EnterpriseContractEntity> enterpriseContractList)
        {
            RemindEntity remindEntity = CreateRemindEntity();
            foreach (EnterpriseContractEntity enterpriseContractEntity in enterpriseContractList)
            {
                remindEntity.RemindTitle = string.Format("企业【{0}】的合同【{1}】将在{2}到期", enterpriseContractEntity.EnterpriseInfo,enterpriseContractEntity.ContractTitle,  enterpriseContractEntity.ContractStopDate.ToShortDateString());
                remindEntity.RemindCategory = RemindCategories.ContractRemindOfEnterprise;
                remindEntity.RemindUrl = string.Empty;
                List<BusinessUser> employeeList = BusinessUserBLL.GetList(UserTypes.Manager, UserStatuses.Normal);
                foreach (BusinessUser item in employeeList)
                {
                    RemindBLL.Instance.Create(item.ExecutorGuid, ExecutorTypes.User, remindEntity);
                }
            }
        }

        protected override string TaskNameInConfig
        {
            get { return "ContractRemindTaskOfEnterprise"; }
        }
    }
}