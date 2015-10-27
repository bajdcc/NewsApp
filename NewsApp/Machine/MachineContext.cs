using NewsApp.Machine.State;
using System;

namespace NewsApp.Machine
{
    public class MachineContext : IMachineContext
    {
        protected IState state;

        public event ErrorHandler OnError;

        public event EventHandler OnFinished;

        public event ProgressHandler OnProgress;

        public MachineContext()
        {
            
        }

        public void Cancel()
        {

        }

        protected virtual IState DecorateForLogging(IState state)
        {
            if (!Util.TraceHelper.Enabled)
            {
                return state;
            }
            return new LoggingStateDecorator(state, this);
        }

        internal void RaiseOnError(string error)
        {
            if (this.OnError != null)
            {
                this.OnError(this, error);
            }
        }

        internal void RaiseOnFinished()
        {
            if (this.OnFinished != null)
            {
                this.OnFinished(this);
            }
        }

        internal void RaiseOnProgress(int progress)
        {
            if (this.OnProgress != null)
            {
                this.OnProgress(this, progress);
            }
        }

        internal virtual void SetState(IState newState)
        {
            if (newState == null)
            {
                throw new ArgumentNullException("newState");
            }
            this.state = this.DecorateForLogging(newState);
            this.state.OnStateEnter();
        }

        public void Start()
        {
            this.state.OnStart();
        }

        public override string ToString()
        {
            return base.ToString();
        }       

        public int RetryCount { get; set; }

        public virtual TimeSpan RetryTimeout { get; set; }

        public object UserContext { get; set; }
    }
}