using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine
{
    public delegate void ErrorHandler(IMachineContext context, string error);

    public delegate void EventHandler(IMachineContext context);

    public delegate void ProgressHandler(IMachineContext context, int progress);

    public delegate void LoggingHandler(IMachineContext context, string msg);

    public interface IMachineContext
    {
        event ErrorHandler OnError;

        event EventHandler OnFinished;

        event ProgressHandler OnProgress;

        event LoggingHandler OnLogging;

        void Log(string msg);

        void Trace(string msg);

        void AddMessage(Message msg);

        int RetryCount { get; set; }

        TimeSpan RetryTimeout { get; set; }

        object UserContext { get; set; }
    }
}
