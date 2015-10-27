using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsIdleState : BaseState
    {
        bool _start = false;
        Util.StaticTimer _timer;

        public NewsIdleState(MachineContext context) : base(context)
        {

        }

        public override void OnStart()
        {
            this._start = true;
            this._timer = new Util.StaticTimer(TimeSpan.FromSeconds(5));
        }

        public override void OnMessage(Message msg)
        {
            if (this._start)
            {
                Trace("Received message: " + msg.ToString());
                this.Context.AddMessage(msg);
            }
        }

        public override void OnTimer()
        {
            if (this._start)
            {
                if (this._timer.IsTimeout())
                {
                    Trace("Idle...");
                    this._timer.Restart();
                }
            }
        }
    }
}
