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
    public class EntryPoint<T> where T : class, IServiceScheduler
    {
        protected IContainer Container { get; set; }

        public void Init(Action<ContainerBuilder> registerDependencies)
        {
            Trace.WriteLine("Register dependencies with Autofac...");
            var builder = new ContainerBuilder();
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new AutofacJobsModule(typeof(T).Assembly));
            builder.RegisterType<T>().AsSelf().SingleInstance();
            registerDependencies(builder);
            Container = builder.Build();

            HostFactory.Run(x =>
            {
                x.Service<T>(s =>
                {
                    s.ConstructUsing(name => Container.Resolve<T>());
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
