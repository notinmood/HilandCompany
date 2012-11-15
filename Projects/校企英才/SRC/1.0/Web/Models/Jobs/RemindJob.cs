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
    public abstract class RemindJob : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            if (this.IsExcuted() == false)
            {
                ExecuteDetails(context);
                Log();
            }
        }

        /// <summary>
        /// Job执行的具体实现
        /// </summary>
        /// <param name="context"></param>
        protected abstract void ExecuteDetails(JobExecutionContext context);

        /// <summary>
        /// Config中配置的系统任务
        /// </summary>
        protected SystemTaskOfDailyExcutorEntity SystemTaskInConfig
        {
            get { return SystemTaskSectionConfig.Instance.SystemTaskOfDailyExcutorList.Find(item => item.Name == this.TaskNameInConfig); }
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

        private void Log()
        {
            LogEntity entity = new LogEntity();
            entity.LogCategory = "Remind";
            entity.LogDate = DateTime.Now;
            entity.Logger = this.TaskNameInConfig;
            entity.LogLevel = LogLevels.Notice.ToString();
            entity.LogMessage = string.Empty;
            entity.LogStatus = Logics.True;
            entity.LogThread = string.Empty;

            LogBLL.Instance.Create(entity);
        }

        /// <summary>
        /// 在config文件中配置的任务名称
        /// </summary>
        protected abstract string TaskNameInConfig
        {
            get;
        }


        private bool IsExcuted()
        {
            Logics logic = LogBLL.Instance.GetLogStatus(TaskNameInConfig, DateTime.Today);
            return Converter.ToBoolean(logic);
        }
    }
}