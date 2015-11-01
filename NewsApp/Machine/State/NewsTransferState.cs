using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine.State
{
    class NewsTransferState : BaseState
    {
        private const int MAX_CONTENT_LENGTH = 100;
        private IState prev;
        private Util.StaticTimer _timer;
        private Queue<NewsMessage> queueMsg;

        public NewsTransferState(MachineContext context, IState prev) : base(context)
        {
            this.prev = prev;
            this._timer = new Util.StaticTimer(TimeSpan.FromSeconds(8));
            this.queueMsg = new Queue<NewsMessage>();
        }

        public override void OnStart()
        {
            base.Start = true;
            Trace("[Transfer] Started");
        }

        public override void OnMessage(NewsMessage msg)
        {
            if (msg.Content.Length > MAX_CONTENT_LENGTH)
                msg.Content = msg.Content.Substring(0, MAX_CONTENT_LENGTH) + "...";
            queueMsg.Enqueue(msg);
        }

        public override void OnReset()
        {
            _timer.SetMinValue();
            queueMsg.Clear();
        }

        public override void OnTimer()
        {
            if (base.Start && this._timer.IsTimeoutOnce())
            {
                this._timer.Restart();
                OnTransfer();
            }
        }

        private void OnTransfer()
        {
            if (prev != null && queueMsg.Count > 0)
            {
                Trace("[Transfer] Moved one message, rest: " + queueMsg.Count);
                prev.OnMessage(queueMsg.Dequeue());
            }
        }
    }
}
