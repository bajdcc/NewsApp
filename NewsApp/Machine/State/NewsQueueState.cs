using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsQueueState : BaseState
    {
        public NewsQueueState(MachineContext context) : base(context)
        {

        }
    }
}
