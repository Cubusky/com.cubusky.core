using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Cubusky
{
    /// <summary>
    /// Synchronizes calls with Unity's Update() method.
    /// </summary>
    public class UpdateSynchronizer : ISynchronizeInvoke
    {
        private readonly Queue<AsyncResult> toExecute = new();
        public bool InvokeRequired => Thread.CurrentThread.ManagedThreadId != 1;

        public UpdateSynchronizer()
        {
            UpdaterService.onUpdate += ProcessQueue;
        }

        ~UpdateSynchronizer()
        {
            UpdaterService.onUpdate -= ProcessQueue;
        }

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            var asyncResult = new AsyncResult()
            {
                method = method,
                args = args,
                IsCompleted = false,
                manualResetEvent = new ManualResetEvent(false),
            };

            if (InvokeRequired)
            {
                lock (toExecute)
                {
                    toExecute.Enqueue(asyncResult);
                }
            }
            else
            {
                asyncResult.Invoke();
                asyncResult.CompletedSynchronously = true;
            }

            return asyncResult;
        }

        public object EndInvoke(IAsyncResult result)
        {
            if (!result.IsCompleted)
            {
                result.AsyncWaitHandle.WaitOne();
            }

            return result.AsyncState;
        }

        public object Invoke(Delegate method, object[] args)
        {
            if (InvokeRequired)
            {
                var asyncResult = BeginInvoke(method, args);
                return EndInvoke(asyncResult);
            }
            else
            {
                return method.DynamicInvoke(args);
            }
        }

        public void ProcessQueue()
        {
            if (Thread.CurrentThread.ManagedThreadId != 1)
            {
                throw new Exception($"must be called from the main thread (called from thread id: {Thread.CurrentThread.ManagedThreadId}.");
            }

            AsyncResult data = null;
            while (true)
            {
                lock (toExecute)
                {
                    if (toExecute.Count == 0)
                    {
                        break;
                    }

                    data = toExecute.Dequeue();
                }

                data.Invoke();
            }
        }

        private class AsyncResult : IAsyncResult
        {
            public Delegate method;
            public object[] args;
            public bool IsCompleted { get; set; }
            public WaitHandle AsyncWaitHandle => manualResetEvent;
            public ManualResetEvent manualResetEvent;
            public object AsyncState { get; set; }
            public bool CompletedSynchronously { get; set; }

            public void Invoke()
            {
                AsyncState = method.DynamicInvoke(args);
                IsCompleted = true;
                manualResetEvent.Set();
            }
        }
    }
}
