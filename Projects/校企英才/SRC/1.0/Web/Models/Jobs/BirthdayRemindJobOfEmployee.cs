using System.Collections.Generic;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.General;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;

namespace XQYC.Web.Models.Jobs
{
    /// <summary>
    /// 内部员工生日提醒
    /// </summary>
    public class BirthdayRemindJobOfEmployee : BirthdayRemindJob
    {
        /// <summary>
        /// 内部员工生日提醒是发送到配置文件中设置的生日提醒接收人角色
        /// </summary>
        /// <param name="birthdayUserList"></param>
        protected override void DispatchRemindMessage(List<BusinessUser> birthdayUserList)
        {
            string[] roles = StringHelper.SplitToArray(SystemTaskInConfig.GetAddonItemValue("receiveRoleNames"));

            RemindEntity remindEntity = CreateRemindEntity();

            foreach (BusinessUser currentUser in birthdayUserList)
            {
                remindEntity.RemindTitle = string.Format("同事【{0}】将在{1}过生日",currentUser.UserNameDisplay,currentUser.UserBirthDay.ToShortDateString());
                remindEntity.RemindCategory = RemindCategories.BirthdayRemindOfEmployee;
                remindEntity.RemindUrl = string.Empty;

                //向指定的角色发送提醒数据
                foreach (string currentRole in roles)
                {
                    BusinessRole role = BusinessRoleBLL.Get(currentRole);
                    RemindBLL.Instance.Create(role.ExecutorGuid, ExecutorTypes.Role, remindEntity);
                }
            }
        }

        protected override string TaskNameInConfig
        {
            get { return "BirthdayRemindTaskOfEmployee"; }
        }

        protected override UserTypes UserType
        {
            get { return UserTypes.Manager; }
        }
    }
}