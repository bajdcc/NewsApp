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
            Trace("[Overlay] Ready for marquee...");
        }

        public override void OnTimer()
        {
            if (base.Start && base.Context.HasMessage())
            {
                var time = base.Context.Marquee();
                Trace(string.Format("[Overlay] Marquee completed, time for waiting: {0:D}ms", (int)time));
                base.Context.SetState(new NewsWaitingState(base.Context, time));
                base.Context.Start();
            }
        }
    }
}
