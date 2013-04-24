using System;
using System.Collections.Generic;
using HiLand.Framework.BusinessCore;
using HiLand.General;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Enums;
using XQYC.Business.BLL;
using XQYC.Business.Entity;

namespace XQYC.Web.Models.Jobs
{
    /// <summary>
    /// 劳务人员生日提醒
    /// </summary>
    public class BirthdayRemindJobOfLabor : BirthdayRemindJob
    {
        /// <summary>
        /// 劳务人员的生日提醒是发送到为其服务的客服人员
        /// </summary>
        /// <param name="birthdayUserList"></param>
        protected override void DispatchRemindMessage(List<BusinessUser> birthdayUserList)
        {
            RemindEntity remindEntity = CreateRemindEntity();

            foreach (BusinessUser currentUser in birthdayUserList)
            {
                remindEntity.RemindTitle = string.Format("劳务人员【{0}】将在{1}过生日",currentUser.UserNameDisplay,currentUser.UserBirthDay.ToShortDateString());
                remindEntity.RemindCategory = RemindCategories.BirthdayRemindOfLabor;
                remindEntity.RemindUrl = string.Empty;

                //向劳务人员的对应的业务人员发送提醒数据
                LaborEntity labor = LaborBLL.Instance.Get(currentUser.UserGuid);
                if (labor.BusinessUserGuid != Guid.Empty)
                {
                    RemindBLL.Instance.Create(labor.ServiceUserGuid, ExecutorTypes.User, remindEntity);
                }
            }
        }

        protected override string TaskNameInConfig
        {
            get { return "BirthdayRemindTaskOfLabor"; }
        }

        protected override UserTypes UserType
        {
            get { return UserTypes.CommonUser; }
        }
    }
}