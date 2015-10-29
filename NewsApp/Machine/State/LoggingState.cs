using NewsApp.Util;

namespace NewsApp.Machine.State
{
    class LoggingState : IState
    {
        private readonly MachineContext context;
        private readonly IState decoratee;

        public LoggingState(IState decoratee, MachineContext context)
        {
            this.decoratee = decoratee;
            this.context = context;
        }

        public string GetStateName()
        {
            return ("[" + this.decoratee.GetType().Name + "]");
        }

        public void OnCancel()
        {
            this.context.Trace(this.GetStateName() + " OnCancel");
            this.decoratee.OnCancel();
        }

        public void OnMessage(NewsMessage msg)
        {
            this.context.Trace(this.GetStateName() + " OnMessage");
            this.decoratee.OnMessage(msg);
        }

        public void OnReset()
        {
            this.context.Trace(this.GetStateName() + " OnReset");
            this.decoratee.OnReset();
        }

        public void OnStart()
        {
            this.context.Trace(this.GetStateName() + " OnStart");
            this.decoratee.OnStart();
        }

        public void OnStateEnter()
        {
            this.context.Trace(this.GetStateName() + " OnStateEnter");
            this.decoratee.OnStateEnter();
        }

        public void OnTimer()
        {
            this.decoratee.OnTimer();
        }
    }
}