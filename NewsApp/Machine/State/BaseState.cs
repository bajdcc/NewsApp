using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class BaseState : IState
    {
        public BaseState(MachineContext context)
        {
            this.Context = context;
        }

        public virtual void OnCancel()
        {
        }

        public virtual void OnMessage(Message msg)
        {
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
    }
}
