using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class QuartzScheduler : IQuartzScheduler
    {
        private Action act;
        private int hour;
        private int min;

        public QuartzScheduler(Action func,int hour,int min)
        {
            this.act = func;
            this.hour = hour;
            this.min = min;
        }

        public async void Setup()
        {
            // construct a scheduler factory
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            // get a scheduler
            IScheduler sched = await factory.GetScheduler();
            await sched.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<Job>()
                .WithIdentity("myJob", "group1")
                .Build();
            job.JobDataMap.Add("action",act);
            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, min))
            .Build();

            await sched.ScheduleJob(job, trigger);
        }

        public void Schedule()
        {
            Setup();
        }
    }

    class Job : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Action act = (Action)context.JobDetail.JobDataMap.Get("action");
            act.Invoke();
        }
    }
}
