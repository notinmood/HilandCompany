using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiLand.General;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using Quartz;
using XQYC.Business.BLL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Web.Models.Jobs
{
    /// <summary>
    /// 劳务人员合同到期提醒
    /// </summary>
    public class ContractRemindJobOfLabor : RemindJob
    {
        protected override void ExecuteDetails(JobExecutionContext context)
        {
            int daysOffToday = 3;

            if (SystemTaskInConfig != null)
            {
                daysOffToday = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("aheadDays"), 7);
            }

            DateTime dateLower = DateTime.Today.AddDays(daysOffToday);
            DateTime dateUpper = dateLower.AddDays(1);

            List<LaborContractEntity> laborContractList = LaborContractBLL.Instance.GetList(string.Format("[LaborContractStatus] ={0} AND LaborContractIsCurrent={1} AND LaborContractStopDate>= '{2}'  AND LaborContractStopDate<'{3}' ", (int)LaborWorkStatuses.Worked,(int)Logics.True, dateLower, dateUpper));
            DispatchRemindMessage(laborContractList);
        }

        private void DispatchRemindMessage(List<LaborContractEntity> laborContractList)
        {
            RemindEntity remindEntity = CreateRemindEntity();
            foreach (LaborContractEntity laborContractEntity in laborContractList)
            {
                LaborEntity labor= laborContractEntity.Labor;
                remindEntity.RemindTitle = string.Format("劳务人员【{0}】的合同【{1}】将在{2}到期", labor.UserNameCN, laborContractEntity.LaborContractDetails, laborContractEntity.LaborContractStopDate.ToShortDateString());
                remindEntity.RemindCategory = RemindCategories.ContractRemindOfLabor;
                remindEntity.RemindUrl = string.Empty;

                RemindBLL.Instance.Create(labor.ServiceUserGuid, ExecutorTypes.User, remindEntity);
            }
        }

        protected override string TaskNameInConfig
        {
            get { return "ContractRemindTaskOfLabor"; }
        }
    }
}