using System;
using HiLand.General;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Setting.SectionHandler;
using Quartz;

namespace XQYC.Web.Models.Jobs
{
    /// <summary>
    /// 提醒类别的系统任务基类
    /// </summary>
    public abstract class RemindJob : SystemJob
    {
        /// <summary>
        /// 日志类别的名称
        /// </summary>
        protected override string LogCategoryName
        {
            get { return "Remind"; }
        }

        /// <summary>
        /// 创建提醒实体
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 请在派生类中修改实体的以下属性
        /// RemindCategory
        /// RemindTitle
        /// RemindGuid
        /// </remarks>
        protected virtual RemindEntity CreateRemindEntity()
        {
            RemindEntity remindEntity = new RemindEntity();
            remindEntity.ActivityKey = string.Empty;
            remindEntity.CreateDate = DateTime.Now;
            remindEntity.Emergency = LevelTypes.Normal;
            remindEntity.ExpireDate = DateTime.Today.AddDays(5);
            remindEntity.Importance = LevelTypes.Normal;
            remindEntity.RemindCategory = RemindCategories.None;
            remindEntity.RemindTitle = string.Empty;
            remindEntity.RemindUrl = string.Empty;
            remindEntity.RemindType = 0;
            remindEntity.ResourceKey = string.Empty;
            remindEntity.SenderKey = GuidHelper.SystemKeyString;
            remindEntity.SenderName = "系统";
            remindEntity.StartDate = DateTime.Now;
            remindEntity.TopLevel = 0;
            return remindEntity;
        }
    }
}