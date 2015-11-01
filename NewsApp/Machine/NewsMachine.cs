using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine
{
    class NewsMachine : TimedoutMachine
    {
        static public NewsSettings Settings { get; set; } = new NewsSettings();

        public NewsMachine()
        {
            base.RetryCount = 12;
            base.RetryTimeout = TimeSpan.FromSeconds(1);
            base.IdleTimer = new Util.StaticTimer(TimeSpan.FromMinutes(10));
        }

        public new void Start()
        {
            base.SetState(new State.NewsIdleState(this));
            base.Start();
        }

        public new void Cancel(bool shutdown)
        {
            base.Cancel(shutdown);
            IdleTimer.SetMinValue();
        }

        public void Reset()
        {
            IdleTimer.SetMinValue();
            stateTransfer.OnReset();
            stateCrawler.OnReset();
        }
    }
}
