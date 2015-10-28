using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsBeginState : BaseState
    {
        private bool openOverlay = false;

        public NewsBeginState(MachineContext context) : base(context)
        {

        }

        public override void OnStart()
        {
            base.Start = true;
            Observable.Start(() =>
            {
                base.Context.OpenOverlay();
            }).Delay(TimeSpan.FromSeconds(1))
            .Subscribe(_ => openOverlay = true);            
        }

        public override void OnTimer()
        {
            if (base.Start)
            {
                if (openOverlay)
                {
                    Trace("!!!");
                    openOverlay = false;
                }
                else
                {
                    Trace("...");
                }
            }
        }
    }
}
