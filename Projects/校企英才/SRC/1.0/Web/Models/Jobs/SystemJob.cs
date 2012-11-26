using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Setting.SectionHandler;
using Quartz;

namespace XQYC.Web.Models.Jobs
{
    public abstract class SystemJob : IJob
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

        

        private void Log()
        {
            LogEntity entity = new LogEntity();
            entity.LogCategory = LogCategoryName;
            entity.LogDate = DateTime.Now;
            entity.Logger = this.TaskNameInConfig;
            entity.LogLevel = LogLevels.Notice.ToString();
            entity.LogMessage = string.Empty;
            entity.LogStatus = Logics.True;
            entity.LogThread = string.Empty;

            LogBLL.Instance.Create(entity);
        }

        /// <summary>
        /// 日志类别的名称
        /// </summary>
        protected abstract string LogCategoryName
        {
            get;
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