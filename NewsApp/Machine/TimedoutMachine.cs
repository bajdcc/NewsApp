using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine
{
    abstract class TimedoutMachine : MachineContext
    {
        private const int FRQ_MSECS = 500;
        private System.Threading.Timer timer;

        public new void Start()
        {
            this.timer = new System.Threading.Timer(
                new System.Threading.TimerCallback(this.TimerCallback),
                null,
                FRQ_MSECS,
                FRQ_MSECS);
            base.Start();
        }

        public override void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
            }
            base.Dispose();
        }

        private void TimerCallback(object state)
        {
            if (this.timer != null)
            {
                base.state.OnTimer();
            }
        }
    }
}
