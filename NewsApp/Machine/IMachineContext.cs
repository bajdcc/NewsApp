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

    public interface IMachineContext
    {
        event ErrorHandler OnError;

        event EventHandler OnFinished;

        event ProgressHandler OnProgress;

        int RetryCount { get; set; }

        TimeSpan RetryTimeout { get; set; }

        object UserContext { get; set; }
    }
}
