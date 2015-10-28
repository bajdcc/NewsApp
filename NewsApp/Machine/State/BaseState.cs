using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class BaseState : IState
    {
        private bool start = false;

        public BaseState(MachineContext context)
        {
            this.Context = context;
        }

        public virtual void OnCancel()
        {
        }

        public virtual void OnMessage(NewsMessage msg)
        {
            Trace("Received message: " + msg.ToString());
            Context.AddMessage(msg);
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnStateEnter()
        {

        }

        public virtual void OnTimer()
        {
        }

        protected MachineContext Context { get; private set; }

        protected bool Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }

        protected void Trace(string msg)
        {
            this.Context.Trace(msg);
        }
    }
}
