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
        private Util.StaticTimer overlayAnimation;
        private Util.StaticTimer waitingForOverlayAnimationCompleted;

        public NewsBeginState(MachineContext context) : base(context)
        {
            waitingForOverlayAnimationCompleted = new Util.StaticTimer(TimeSpan.FromSeconds(1));
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
                    Trace("[overlay]Created window");
                    overlayAnimation = new Util.StaticTimer(TimeSpan.FromSeconds(9));
                    openOverlay = false;
                }
                if (overlayAnimation != null && overlayAnimation.IsTimeout())
                {
                    base.Context.SetState(new NewsQueueState(base.Context));
                    base.Context.Start();
                }
                else if (waitingForOverlayAnimationCompleted.IsTimeout())
                {
                    Trace("[overlay]Waiting for animation completed...");
                    waitingForOverlayAnimationCompleted.Restart();
                }
            }
        }
    }
}
