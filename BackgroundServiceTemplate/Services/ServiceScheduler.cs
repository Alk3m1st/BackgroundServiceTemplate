using Autofac;
using Autofac.Extras.Quartz;
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

        public ServiceScheduler(ISchedulerFactory schedulerFactory)
        {
            SchedulerFactory = schedulerFactory;
        }

        public void Start()
        {
            Trace.WriteLine("Service scheduler starting...");
            try
            {
                Scheduler = SchedulerFactory.GetScheduler();

                Trace.WriteLine("Firing triggers...");
                Scheduler.Start();

                Trace.WriteLine("Do stuff");

                Scheduler.Shutdown();
            }
            catch(TaskSchedulerException se)
            {
                Trace.WriteLine("Service failed to start");
                Trace.WriteLine(se);
            }
        }

        public void Stop()
        {
            Trace.WriteLine("Service scheduler starting...");
        }
    }
}