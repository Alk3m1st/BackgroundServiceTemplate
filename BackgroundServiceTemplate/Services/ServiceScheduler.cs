using Autofac;
using Autofac.Extras.Quartz;
using BackgroundServiceTemplate.Interfaces;
using BackgroundServiceTemplate.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundServiceTemplate.Services
{
    public class ServiceScheduler
    {
        public IScheduler Scheduler { get; set; }
        public ISchedulerFactory SchedulerFactory { get; set; }
        public IEnumerable<BackgroundServiceJob> Jobs { get; set; }

        public ServiceScheduler(ISchedulerFactory schedulerFactory, IEnumerable<BackgroundServiceJob> jobs)
        {
            SchedulerFactory = schedulerFactory;
            Jobs = jobs;
        }

        public void Start()
        {
            Trace.WriteLine("Service scheduler starting...");
            try
            {
                Scheduler = SchedulerFactory.GetScheduler();

                Trace.WriteLine("Firing triggers...");
                Scheduler.Start();

                Trace.WriteLine("Schedule Jobs");
                foreach(var job in Jobs)
                {
                    var jobDetail = JobBuilder.Create(job.GetType())
                        .WithIdentity(job.GetType().Name, "group1")
                        .Build();

                    var tb = TriggerBuilder.Create()
                        .WithIdentity(job.GetType().Name, "group1")
                        .StartNow();
                    var trigger = job.GetSchedule(tb).Build();

                    Scheduler.ScheduleJob(jobDetail, trigger);
                }

                Trace.WriteLine("Do stuff");
            }
            catch(TaskSchedulerException se)
            {
                Trace.WriteLine("Service failed to start");
                Trace.WriteLine(se);
            }
        }

        public void Stop()
        {
            Trace.WriteLine("Service scheduler stoping...");
            Scheduler.Shutdown();
        }
    }
}