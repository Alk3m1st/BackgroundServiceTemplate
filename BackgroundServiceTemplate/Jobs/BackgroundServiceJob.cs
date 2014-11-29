using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundServiceTemplate.Interfaces
{
    public abstract class BackgroundServiceJob : IJob
    {
        public abstract TriggerBuilder GetSchedule(TriggerBuilder triggerBuilder);
        protected abstract void ExecuteInner(IJobExecutionContext context);

        public void Execute(IJobExecutionContext context)
        {
            ExecuteInner(context);
        }
    }
}
