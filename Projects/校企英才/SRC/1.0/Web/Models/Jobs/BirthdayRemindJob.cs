using System;
using System.Collections.Generic;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using Quartz;

namespace XQYC.Web.Models.Jobs
{
    /// <summary>
    /// 生日类提醒
    /// </summary>
    public abstract class BirthdayRemindJob : RemindJob
    {
        /// <summary>
        /// Job执行的具体实现
        /// </summary>
        /// <param name="context"></param>
        protected override void ExecuteDetails(JobExecutionContext context)
        {
            int daysOffToday = 3;

            if (SystemTaskInConfig != null)
            {
                daysOffToday = Converter.ChangeType(SystemTaskInConfig.GetAddonItemValue("aheadDays"), 3);
            }

            DateTime dateLower = DateTime.Today.AddDays(daysOffToday);
            DateTime dateUpper = DateTime.Today.AddDays(daysOffToday + 1);

            List<BusinessUser> userList = BusinessUserBLL.GetList(string.Format("[UserType] ={0} AND [UserBirthDay]>= '{1}'  AND [UserBirthDay]<='{2}' ", (int)this.UserType, dateLower, dateUpper));
            DispatchRemindMessage(userList);
        }

        /// <summary>
        /// 发送提醒消息
        /// </summary>
        /// <param name="userList"></param>
        protected abstract void DispatchRemindMessage(List<BusinessUser> userList);

        /// <summary>
        /// 目标体的用户类型
        /// </summary>
        protected abstract UserTypes UserType
        {
            get;
        }
    }
}