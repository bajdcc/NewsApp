using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NewsApp.Machine
{
    public delegate void ErrorHandler(IMachineContext context, string error);

    public delegate void FinishedHandler(IMachineContext context);

    public delegate void ProgressHandler(IMachineContext context, int progress);

    public delegate void LoggingHandler(IMachineContext context, string msg);

    public delegate double MarqueeHandler(IMachineContext context, NewsMessage msg);

    public interface IMachineContext
    {
        event ErrorHandler OnError;

        event FinishedHandler OnFinished;

        event ProgressHandler OnProgress;

        event LoggingHandler OnLogging;

        event MarqueeHandler OnMarquee;

        void Log(string msg);

        void Trace(string msg);

        void AddMessage(NewsMessage msg);

        bool HasMessage();

        void OpenOverlay();

        void CloseOverlay();

        double Marquee();

        MainOverlay Overlay { get; }

        int RetryCount { get; }

        TimeSpan RetryTimeout { get; }

        object UserContext { get; }

        Dispatcher MainDispatcher { get; set; }

        Util.StaticTimer IdleTimer { get; }
    }
}
