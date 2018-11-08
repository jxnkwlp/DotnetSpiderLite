using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DotnetSpiderLite.Infrastructure
{
    public class AutomicLong
    {
        private long _value;

        public long Value => _value;

        public AutomicLong(long value)
        {
            _value = value;
        }

        public long Increment()
        {
            return Interlocked.Increment(ref _value);
        }

        public long Decrement()
        {
            return Interlocked.Decrement(ref _value);
        }

        public long Set(int value)
        {
            return Interlocked.Exchange(ref _value, value);
        }
    }
}
