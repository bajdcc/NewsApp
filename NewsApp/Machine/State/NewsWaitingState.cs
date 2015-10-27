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

        public NewsWaitingState(MachineContext context) : base(context)
        {
            this.timer = new Util.StaticTimer(context.RetryTimeout);
            this.retriesUsed = 0;
        }

        private void HandleTimeout()
        {
            Trace("Idle...");
        }

        public override void OnTimer()
        {
            if (this.timer.IsTimeout())
            {
                this.timer.Restart();
                if (this.retriesUsed++ >= base.Context.RetryCount)
                {
                    base.Context.SetState(new NewsEndState(base.Context));
                }
                else
                {
                    this.HandleTimeout();
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
