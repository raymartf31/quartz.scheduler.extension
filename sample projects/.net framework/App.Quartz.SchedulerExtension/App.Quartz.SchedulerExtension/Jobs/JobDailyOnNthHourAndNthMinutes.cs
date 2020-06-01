using System;
using System.Threading.Tasks;

using Quartz;

namespace App.Quartz.SchedulerExtension.Jobs
{
    public class JobDailyOnNthHourAndNthMinutes : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"JobDailyOnNthHourAndNthMinutes executed at: {DateTime.Now}");

            return Task.CompletedTask;
        }
    }
}
