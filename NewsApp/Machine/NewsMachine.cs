﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Machine
{
    class NewsMachine : TimedoutMachine
    {
        public NewsMachine()
        {
            base.RetryCount = 5;
            base.RetryTimeout = TimeSpan.FromSeconds(1);
            base.IdleTimer = new Util.StaticTimer(TimeSpan.FromSeconds(5));
        }

        public new void Start()
        {
            base.SetState(new State.NewsIdleState(this));
            base.Start();
        }
    }
}
