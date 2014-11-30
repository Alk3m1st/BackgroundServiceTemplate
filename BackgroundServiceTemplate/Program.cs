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
        public static void Main()
        {
            var entryBase = new EntryPoint<ServiceScheduler>();
            entryBase.Init(RegisterDependencies);
        }

        public static void RegisterDependencies(ContainerBuilder builder)
        {
            // TODO: Add further dependencies
        }
    }
}
