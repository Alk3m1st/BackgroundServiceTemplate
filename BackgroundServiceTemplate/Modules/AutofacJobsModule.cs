using Autofac;
using BackgroundServiceTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundServiceTemplate.Modules
{
    public class AutofacJobsModule : Autofac.Module
    {
        private readonly Assembly[] _jobsAssembly;

        public AutofacJobsModule(params Assembly[] jobsAssembly)
        {
            _jobsAssembly = jobsAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_jobsAssembly)
                .Where(type => !type.IsAbstract && typeof(BackgroundServiceJob).IsAssignableFrom(type))
                .AsSelf()
                .As<BackgroundServiceJob>()
                .InstancePerLifetimeScope();
        }
    }
}
