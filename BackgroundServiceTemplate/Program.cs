using Autofac;
using Autofac.Extras.Quartz;
using BackgroundServiceTemplate.Modules;
using BackgroundServiceTemplate.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace BackgroundServiceTemplate
{
    public class Program
    {
        public static IContainer Container { get; set; }

        public static void Main()
        {
            Trace.WriteLine("Register dependencies with Autofac...");
            var builder = new ContainerBuilder();
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new AutofacJobsModule(typeof(ServiceScheduler).Assembly));   // Change with type T
            builder.RegisterType<ServiceScheduler>().AsSelf().SingleInstance();
            Container = builder.Build();

            HostFactory.Run(x =>
            {
                x.Service<ServiceScheduler>(s =>
                {
                    s.ConstructUsing(name => Container.Resolve<ServiceScheduler>());
                    s.WhenStarted(ss => ss.Start());
                    s.WhenStopped(ss => ss.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Background service Topshelf host");
                x.SetDisplayName("BackgroundServiceTemplate");
                x.SetServiceName("BackgroundServiceTemplate");
                x.SetInstanceName(Environment.MachineName);

                x.EnableServiceRecovery(rc => rc.RestartService(1));    // Restart the service after 1 minute if it crashed
            });
        }
    }
}
