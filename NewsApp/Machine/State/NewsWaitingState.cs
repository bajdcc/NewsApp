using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsWaitingState : BaseState
    {
        private int retriesUsed;
        private readonly Util.StaticTimer timer;
        private readonly Util.StaticTimer queueTime;

        public NewsWaitingState(MachineContext context, double time) : base(context)
        {
            this.timer = new Util.StaticTimer(context.RetryTimeout);
            this.queueTime = new Util.StaticTimer(TimeSpan.FromMilliseconds(time));
            this.retriesUsed = 0;
        }

        private void HandleTimeout()
        {
            Trace("Idle...");
        }

        public override void OnMessage(NewsMessage msg)
        {
            base.OnMessage(msg);
            ResetTimeout();
        }

        public override void OnTimer()
        {
            if (queueTime.IsTimeoutOnce())
            {
                if (base.Context.HasMessage())
                {
                    Trace("Ready for queue...");
                    base.Context.SetState(new NewsQueueState(base.Context));
                    base.Context.Start();
                }
                else
                {
                    if (this.timer.IsTimeout())
                    {
                        this.timer.Restart();
                        if (this.retriesUsed++ >= base.Context.RetryCount)
                        {
                            Trace("No messages available");
                            base.Context.SetState(new NewsEndState(base.Context));
                            base.Context.Start();
                        }
                        else
                        {
                            this.HandleTimeout();
                        }
                    }
                }
            }            
        }

        protected void ResetTimeout()
        {
            this.timer.Restart();
            this.retriesUsed = 0;
        }
    }
}
