using System;
using System.Threading.Tasks;

using Quartz;

namespace App.Quartz.SchedulerExtension.Jobs
{
    public class JobWithSpecificTime : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"JobWithSpecificTime executed at: {DateTime.Now}");

            return Task.CompletedTask;
        }
    }
}
