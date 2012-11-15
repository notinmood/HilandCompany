using System;
using HiLand.Utility.Data;
using HiLand.Utility.Event;
using HiLand.Utility.Setting;
using HiLand.Utility.Setting.SectionHandler;
using Quartz;
using Quartz.Impl;
using XQYC.Web.Models.Jobs;

namespace XQYC.Web.Models
{
    public class JobContainer
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="scheduler"></param>
        /// <remarks>
        /// </remarks>
        public static void ExecuteJobs(IScheduler scheduler)
        {
            SystemTaskSectionConfig sectionConfig = Config.GetSection<SystemTaskSectionConfig>("systemTaskSection");

            foreach (SystemTaskOfDailyExcutorEntity item in sectionConfig.SystemTaskOfDailyExcutorList)
            {
                JobDetail jobDetail = new JobDetail(item.Name + "Job", null, item.Type);

                Trigger triger = TriggerUtils.MakeDailyTrigger(item.ExcuteHour, item.ExcuteMinute);
                triger.Name = item.Name + "Triger";

                //0.由于采用了2种方式执行job，为了防止对同一个job可能多次的执行，就需要在job内部自己控制重复的逻辑。

                //1.如果系统一直未退出应用程序域，那么就可以使用Schedule来执行
                if (scheduler != null)
                {
                    scheduler.ScheduleJob(jobDetail, triger);
                }

                //2.如果系统在退出应用程序域的时间段内已经过了task执行的时间，那么手动执行task
                CommonHandle<SystemTaskOfDailyExcutorEntity> commonHandle = new CommonHandle<SystemTaskOfDailyExcutorEntity>(ExcuteJobManaul);
                commonHandle.BeginInvoke(item, null, null);
            }
        }

        /// <summary>
        /// 手工调试Job使用
        /// </summary>
        public static void DebugJobs()
        {
            SystemTaskSectionConfig sectionConfig = Config.GetSection<SystemTaskSectionConfig>("systemTaskSection");

            foreach (SystemTaskOfDailyExcutorEntity item in sectionConfig.SystemTaskOfDailyExcutorList)
            {
                ExcuteJobManaul(item);
            }
        }

        /// <summary>
        /// 手动执行task
        /// </summary>
        /// <param name="item"></param>
        private static void ExcuteJobManaul(SystemTaskOfDailyExcutorEntity item)
        {
            DateTime scheduledTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.ExcuteHour, item.ExcuteMinute, 0);
            if (DateTime.Now > scheduledTime)
            {
                IJob job = TypeHelper.Activate<IJob>(item.Type);
                if (job != null)
                {
                    job.Execute(null);
                }
            }
        }
    }
}