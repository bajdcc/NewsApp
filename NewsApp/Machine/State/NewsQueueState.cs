using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsQueueState : BaseState
    {
        public NewsQueueState(MachineContext context) : base(context)
        {

        }

        public override void OnStart()
        {
            base.Start = true;
            Trace("Ready for marquee...");
        }

        public override void OnTimer()
        {
            if (base.Start && base.Context.HasMessage())
            {
                var time = base.Context.Marquee();
                Trace("Marquee completed");
                base.Context.SetState(new NewsWaitingState(base.Context, time));
                base.Context.Start();
            }
        }
    }
}
