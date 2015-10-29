using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsIdleState : BaseState
    {
        private Util.StaticTimer _timer;

        public NewsIdleState(MachineContext context) : base(context)
        {

        }

        public override void OnStart()
        {
            base.Start = true;
            this._timer = new Util.StaticTimer(TimeSpan.FromSeconds(5));
        }

        public override void OnMessage(NewsMessage msg)
        {
            if (base.Start)
            {
                base.OnMessage(msg);
            }
        }

        public override void OnTimer()
        {
            if (base.Start)
            {
                if (this._timer.IsTimeout())
                {
                    Trace("Idle...");
                    this._timer.Restart();
                }

                if (base.Context.IdleTimer.IsTimeoutOnce())
                {
                    if (base.Context.HasMessage())
                    {
                        base.Context.IdleTimer.Restart();
                        base.Context.SetState(new NewsBeginState(base.Context));
                        base.Context.Start();
                    }
                }
            }
        }
    }
}
