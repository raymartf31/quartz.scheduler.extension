using System;
using System.Threading.Tasks;

using Quartz;

namespace App.Quartz.SchedulerExtension.Jobs
{
    public class JobOnNthMinutesOfAnHour : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"JobOnNthMinutesOfAnHour executed at: {DateTime.Now}");

            return Task.CompletedTask;
        }
    }
}
