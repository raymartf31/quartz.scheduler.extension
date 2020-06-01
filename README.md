# quartz.scheduler.extension

This is an extension component for <a href="https://www.quartz-scheduler.net/">quartz</a> scheduler with C#.

This component contains 3 major methods:

1. ScheduleJobWithSpecificTime - This schedules a job to fire on specific time of the day.
2. ScheduleJobOnNthMinutesOfAnHour - This schedules a job to fire on nth minute of every hour. Example: Scheduled interval is 15 minutes, the job will be fired every 15 minutes of an hour: 1:15, 1:30, 1:45, 2:00...
3. ScheduleJobDailyOnNthHourAndNthMinutes - This schedules a job to fire on nth hours and nth minutes of every day. Example: Configured schedule is 2 hrs and 30 minutes, the job will then get triggered at 2:30, 4:30, 6:30, 8:30...
