using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsIdleState : BaseState
    {
        Util.StaticTimer _timer;

        public NewsIdleState(MachineContext context) : base(context)
        {

        }

        public override void OnStart()
        {
            base.Start = true;
            this._timer = new Util.StaticTimer(TimeSpan.FromSeconds(5));
        }

        public override void OnMessage(Message msg)
        {
            if (base.Start)
            {
                base.OnMessage(msg);
                base.Context.SetState(new NewsBeginState(base.Context));
                base.Context.Start();
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

                    OnMessage(new Message() { Origin = "example", Content = "ni hao" });
                }
                else
                {

                }
            }
        }
    }
}
