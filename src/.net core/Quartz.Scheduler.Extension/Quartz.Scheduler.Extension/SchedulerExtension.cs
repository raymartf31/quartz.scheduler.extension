using System;
using System.Collections.Generic;

namespace Quartz.Scheduler.Extension
{
    public static class SchedulerExtension
    {
        public static void ScheduleJobWithSpecificTime<TJob>(
            this IScheduler scheduler, string group, string jobName, DateTime schedule, IDictionary<string, object> dataMap = null)
            where TJob : IJob
        {
            if (dataMap == null)
                dataMap = new Dictionary<string, object>();

            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName, group)
                .StoreDurably(true) //Whether or not the job should remain stored after it is orphaned
                .SetJobData(new JobDataMap(dataMap))
                .Build();

            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(jobName + "SpecificTimeTrigger", group)
                .WithSchedule(
                    CronScheduleBuilder.DailyAtHourAndMinute(schedule.Hour, schedule.Minute)
                        .WithMisfireHandlingInstructionFireAndProceed() // Immediately executes first misfired execution and discards other (i.e. all misfired executions are merged together). Then back to schedule. No matter how many trigger executions were missed, only single immediate execution is performed.
                );

            var trigger = triggerBuilder.Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void ScheduleJobOnNthMinutesOfAnHour<TJob>(
            this IScheduler scheduler, string group, string jobName, int minutes, IDictionary<string, object> dataMap = null)
            where TJob : IJob
        {
            if (dataMap == null)
                dataMap = new Dictionary<string, object>();

            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName, group)
                .StoreDurably(true) //Whether or not the job should remain stored after it is orphaned
                .SetJobData(new JobDataMap(dataMap))
                .Build();

            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(jobName + "NthMinutesOfAnHourTrigger", group)
                .WithSchedule(BuildToRunOnNthMinutesOfAnHour(minutes));

            var trigger = triggerBuilder.Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void ScheduleJobDailyOnNthHourAndNthMinutes<TJob>(
            this IScheduler scheduler, string group, string jobName, int hours, int minutes, IDictionary<string, object> dataMap = null)
            where TJob : IJob
        {
            if (dataMap == null)
                dataMap = new Dictionary<string, object>();

            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName, group)
                .StoreDurably(true) //Whether or not the job should remain stored after it is orphaned
                .SetJobData(new JobDataMap(dataMap))
                .Build();

            // Multiple triggers to fire at exact moments of the day.
            var triggers = new HashSet<ITrigger>();

            var executionHours = GetExecutionHours(hours);

            foreach (var executionHour in executionHours)
            {
                var trigger = TriggerBuilder.Create()
                    .WithIdentity(jobName + "DailyOnNthHourAndNthMinutesTrigger" + executionHour, group)
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(executionHour, minutes))
                    .Build();

                triggers.Add(trigger);
            }

            scheduler.ScheduleJob(job, triggers, false);
        }

        private static IScheduleBuilder BuildToRunOnNthMinutesOfAnHour(int minutes)
        {
            // http://www.quartz-scheduler.org/documentation/quartz-2.3.0/tutorials/tutorial-lesson-06.html
            var cronExpression = $"0 0/{minutes} * * * ?";

            return CronScheduleBuilder.CronSchedule(cronExpression)
                .WithMisfireHandlingInstructionDoNothing();
        }

        private static SortedSet<int> GetExecutionHours(int hoursInterval)
        {
            var totalHoursInADay = 24;
            var numberOfExecutions = Convert.ToInt32(totalHoursInADay / hoursInterval);
            var executionHours = new SortedSet<int>();

            for (int i = 1; i <= numberOfExecutions; i++)
            {
                var executionHour = i * hoursInterval;
                executionHour = executionHour == totalHoursInADay ? 0 : executionHour;

                if (executionHour > totalHoursInADay)
                {
                    break;
                }

                executionHours.Add(executionHour);
            }

            return executionHours;
        }
    }
}
