﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Util
{
    public class StaticTimer
    {
        private DateTime nextTimeout;
        private readonly TimeSpan timeout;

        public StaticTimer(TimeSpan timeout)
        {
            this.timeout = timeout;
            this.Restart();
        }

        public bool IsTimeout()
        {
            bool flag = DateTime.Now >= this.nextTimeout;
            if (flag)
            {
                this.nextTimeout = DateTime.MaxValue;
            }
            return flag;
        }

        public bool IsTimeoutOnce()
        {
            return DateTime.Now >= this.nextTimeout;
        }

        public void Restart()
        {
            this.nextTimeout = DateTime.Now.Add(this.timeout);
        }

        public void SetMaxValue()
        {
            this.nextTimeout = DateTime.MaxValue;
        }

        public void SetMinValue()
        {
            this.nextTimeout = DateTime.MinValue;
        }
    }
}
