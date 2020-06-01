using System;
using System.Threading.Tasks;

using App.Quartz.SchedulerExtension.Jobs;

using Quartz.Impl;
using Quartz.Scheduler.Extension;

namespace App.Quartz.SchedulerExtension
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var scheduler = await StdSchedulerFactory.GetDefaultScheduler();

                var group = "QuartzExtension";

                var schedule = DateTime.Now.AddMinutes(3);

                scheduler.ScheduleJobWithSpecificTime<JobWithSpecificTime>(
                    group, "Job1", schedule);

                scheduler.ScheduleJobOnNthMinutesOfAnHour<JobOnNthMinutesOfAnHour>(group, "Job2", 2);

                scheduler.ScheduleJobDailyOnNthHourAndNthMinutes<JobDailyOnNthHourAndNthMinutes>(group, "Job3", 22, 50);

                await scheduler.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}
