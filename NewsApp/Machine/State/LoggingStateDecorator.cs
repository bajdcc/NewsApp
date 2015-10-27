using NewsApp.Util;

namespace NewsApp.Machine.State
{
    class LoggingStateDecorator : IState
    {
        private readonly MachineContext context;
        private readonly IState decoratee;

        public LoggingStateDecorator(IState decoratee, MachineContext context)
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
            TraceHelper.Trace(this.GetStateName() + " OnCancel: ", this.context);
            this.decoratee.OnCancel();
        }

        public void OnMessage(Message msg)
        {
            TraceHelper.Trace(this.GetStateName() + " OnMessage: " +　msg, this.context);
            this.decoratee.OnMessage(msg);
        }

        public void OnStart()
        {
            TraceHelper.Trace(this.GetStateName() + " OnStart: ", this.context);
            this.decoratee.OnStart();
        }

        public void OnStateEnter()
        {
            TraceHelper.Trace(this.GetStateName() + " OnStateEnter: ", this.context);
            this.decoratee.OnStateEnter();
        }

        public void OnTimer()
        {
            this.decoratee.OnTimer();
        }
    }
}