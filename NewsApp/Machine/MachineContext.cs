using NewsApp.Machine.State;
using System;
using System.Collections.Generic;

namespace NewsApp.Machine
{
    public class MachineContext : IMachineContext, IDisposable
    {
        protected IState state;

        public event ErrorHandler OnError;

        public event EventHandler OnFinished;

        public event ProgressHandler OnProgress;

        public event LoggingHandler OnLogging;

        protected Queue<Message> queue;

        public MachineContext()
        {
            queue = new Queue<Message>();
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
            return new LoggingState(state, this);
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

        public void AddMessage(Message msg)
        {
            queue.Enqueue(msg);
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

        public void Log(string msg)
        {
            if (OnLogging != null)
            {
                OnLogging(this, msg);
            }
        }

        public void Trace(string msg)
        {
            Util.TraceHelper.Trace(this, msg);
        }

        public override string ToString()
        {
            return "Queue#" + queue.Count;
        }

        public virtual void Dispose()
        {

        }

        public int RetryCount { get; set; }

        public virtual TimeSpan RetryTimeout { get; set; }

        public object UserContext { get; set; }
    }
}