using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DotnetSpiderLite.Infrastructure
{
    /// <summary>
    ///  thraed safe change value 
    /// </summary>
    public class AtomicInteger
    {
        private int _value;

        public int Value => _value;

        public AtomicInteger(int value)
        {
            _value = value;
        }

        public int Increment()
        {
            return Interlocked.Increment(ref _value);
        }

        public int Decrement()
        {
            return Interlocked.Decrement(ref _value);
        }

        public int Set(int value)
        {
            return Interlocked.Exchange(ref _value, value);
        }

    }
}
