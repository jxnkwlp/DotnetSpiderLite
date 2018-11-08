using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetSpiderLite
{
    internal static class AsyncContext
    {
        public static void Run(Func<Task> task)
        {
            using (new ContextScope())
            {
                Task.Run(async () => await task()).GetAwaiter().GetResult();
            }
        }

        private class ContextScope : IDisposable
        {
            private readonly SynchronizationContext _current;

            public ContextScope()
            {
                _current = SynchronizationContext.Current;
                SynchronizationContext.SetSynchronizationContext(null);
            }

            public void Dispose()
            {
                SynchronizationContext.SetSynchronizationContext(_current);
            }
        }
    }
}
