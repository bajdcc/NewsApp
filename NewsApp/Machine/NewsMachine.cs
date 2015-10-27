using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine
{
    class NewsMachine : TimedoutMachine
    {
        public NewsMachine()
        {
            this.RetryCount = 5;
            this.RetryTimeout = TimeSpan.FromSeconds(1);
        }

        public new void Start()
        {
            this.SetState(new State.NewsIdleState(this));
            base.Start();
        }
    }
}
