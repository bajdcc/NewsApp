﻿using NewsApp.Machine.State;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace NewsApp.Machine
{
    public class MachineContext : IMachineContext, IDisposable
    {
        protected IState state;

        protected IState stateTransfer;

        protected IState stateCrawler;

        public event ErrorHandler OnError;

        public event FinishedHandler OnFinished;

        public event ProgressHandler OnProgress;

        public event LoggingHandler OnLogging;

        public event MarqueeHandler OnMarquee;

        protected Queue<NewsMessage> queue;

        private MainOverlay overlay;

        private Dispatcher dispatcher;

        public MachineContext()
        {
            this.queue = new Queue<NewsMessage>();
        }        

        public void Cancel(bool shutdown)
        {
            this.state.OnCancel(shutdown);
        }

        protected virtual IState DecorateForLogging(IState state)
        {
            if (!Util.TraceHelper.Enabled)
            {
                return this.state;
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

        public void AddMessage(NewsMessage msg)
        {
            this.queue.Enqueue(msg);
        }

        public bool HasMessage()
        {
            return queue.Count > 0;
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
            if (this.stateTransfer == null)
            {
                this.stateTransfer = new NewsTransferState(this, this.state);
                this.stateTransfer.OnStart();
            }
            if (this.stateCrawler == null)
            {
                this.stateCrawler = new NewsCrawlerState(this, this.stateTransfer);
                this.stateCrawler.OnStart();
            }
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
            OnMarquee = null;
            await MainDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                var overlay = new MainOverlay();
                overlay.Background = System.Windows.Media.Brushes.Transparent;
                var mainOverlayHeight = (double)App.Current.Resources["MainOverlayHeight"];
                overlay.Top = SystemParameters.WorkArea.Height - mainOverlayHeight;
                overlay.Left = SystemParameters.WorkArea.Left;
                overlay.Show();
                OnMarquee += overlay.Marquee;
                this.overlay = overlay;
            }));
        }

        public async void CloseOverlay(bool shutdown)
        {
            if (this.overlay == null)
                return;
            var overlay = this.overlay;
            this.overlay = null;
            await MainDispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                overlay.Resources["UseBackupAnimation"] = !shutdown;
                overlay.Close();
                overlay = null;
            }));
        }

        public double Marquee()
        {
            if (OnMarquee != null)
            {
                return OnMarquee(this, queue.Dequeue());
            }
            return 0;
        }

        public int RetryCount { get; protected set; }

        public TimeSpan RetryTimeout { get; protected set; }

        public object UserContext { get; protected set; }

        public Util.StaticTimer IdleTimer { get; protected set; }

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