using NewsApp.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Util
{
    static public class TraceHelper
    {
        public static bool Enabled { set; get; }
        static TraceHelper()
        {
            Enabled = true;
        }

        internal static void Trace(string message, IMachineContext context)
        {
            if (Enabled)
            {
                MainWindow.TraceOutput(string.Join(context.ToString(), ": ",message));
            }
        }

        internal static void Trace(string message)
        {
            if (Enabled)
            {
                MainWindow.TraceOutput(message);
            }
        }
    }
}
