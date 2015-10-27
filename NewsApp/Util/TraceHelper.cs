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

        internal static void Trace(IMachineContext context, string message)
        {
            if (Enabled)
            {
                context.Log(string.Format("{0} {1} {2}", DateTime.Now.ToLocalTime(), context.ToString(), message));
            }
        }
    }
}
