using NewsApp.Machine.State;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace NewsApp.Machine
{
    public class MachineContext : IMachineContext, IDisposable
    {
        protected IState state;

        public event ErrorHandler OnError;

        public event FinishedHandler OnFinished;

        public event ProgressHandler OnProgress;

        public event LoggingHandler OnLogging;

        protected Queue<Message> queue;

        private MainOverlay overlay;

        private Dispatcher dispatcher;

        public MachineContext()
        {
            queue = new Queue<Message>();
        }        

        public void Cancel()
        {
            Dispose();
            overlay.Close();
            overlay = null;
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

        public async void OpenOverlay()
        {
            await MainDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                var overlay = new MainOverlay();
                overlay.Background = System.Windows.Media.Brushes.Transparent;
                var mainOverlayHeight = (double)App.Current.Resources["MainOverlayHeight"];
                overlay.Top = SystemParameters.WorkArea.Height - mainOverlayHeight;
                overlay.Left = SystemParameters.WorkArea.Left;
                overlay.Height = mainOverlayHeight;
                overlay.Width = SystemParameters.WorkArea.Width;
                overlay.Show();
                this.overlay = overlay;
            }));
        }

        public void CloseOverlay()
        {
            
        }

        public int RetryCount { get; set; }

        public virtual TimeSpan RetryTimeout { get; set; }

        public object UserContext { get; set; }

        public MainOverlay Overlay
        {
            get
            {
                return overlay;
            }
        }

        public Dispatcher MainDispatcher
        {
            get
            {
                return dispatcher;
            }

            set
            {
                if (dispatcher == null)
                    dispatcher = value;
            }
        }
    }
}