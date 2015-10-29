using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsEndState : BaseState
    {
        private Util.StaticTimer _timer;
        private Util.StaticTimer waitingForOverlayAnimationCompleted;

        public NewsEndState(MachineContext context) : base(context)
        {
            _timer = new Util.StaticTimer(TimeSpan.FromSeconds(1));
        }

        public override void OnStart()
        {
            base.Start = true;
            this.waitingForOverlayAnimationCompleted = new Util.StaticTimer(TimeSpan.FromSeconds(10));
            Trace("[Overlay] Waiting for close...");
            base.Context.CloseOverlay();
        }

        public override void OnTimer()
        {
            if (base.Start)
            {
                if (this.waitingForOverlayAnimationCompleted.IsTimeout())
                {
                    Trace("==> Idle <==");                    
                    base.Context.SetState(new NewsIdleState(base.Context));
                    base.Context.Start();
                }
                else if (_timer.IsTimeout())
                {
                    if (base.Context.HasMessage())
                    {
                        Trace("[Overlay] Waiting for animation completed...");
                    }
                }
            }
        }
    }
}
