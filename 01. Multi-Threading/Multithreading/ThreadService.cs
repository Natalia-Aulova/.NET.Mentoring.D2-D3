using System;
using System.Collections.Generic;
using System.Threading;

namespace Multithreading
{
    public class ThreadService
    {
        public void UseThreads(Action<string> resultAction)
        {
            Perform(10, resultAction);
        }

        public void UseThreadPool(Action<string> resultAction)
        {
            var iterationSemaphore = new Semaphore(1, 1);
            var resultSemaphore = new Semaphore(0, 1);
            PerformViaPool(10, iterationSemaphore, resultSemaphore, resultAction);
            resultSemaphore.WaitOne();
        }

        public void CreateCollection(Action<string> resultAction)
        {
            var list = new List<int>();
            var allowWriteEvent = new AutoResetEvent(true);
            var allowReadEvent = new AutoResetEvent(false);

            var writeThread = new Thread(() => InsertElement(list, allowReadEvent, allowWriteEvent));
            writeThread.Start();

            var readThread = new Thread(() => CheckCollection(list, allowReadEvent, allowWriteEvent, resultAction));
            readThread.Start();

            writeThread.Join();
            readThread.Join();
        }

        private void Perform(int state, Action<string> resultAction)
        {
            Interlocked.Decrement(ref state);
            resultAction($"Thread: {Thread.CurrentThread.ManagedThreadId},\tstate: {state}");

            if (state <= 0) return;

            var thread = new Thread(() => Perform(state, resultAction));
            thread.Start();
            thread.Join();
        }

        private void PerformViaPool(int state, Semaphore iterationSemaphore, Semaphore resultSemaphore, Action<string> resultAction)
        {
            iterationSemaphore.WaitOne();

            state--;
            resultAction($"Thread: {Thread.CurrentThread.ManagedThreadId},\tstate: {state}");

            if (state <= 0)
            {
                resultSemaphore.Release();
                return;
            }

            ThreadPool.QueueUserWorkItem((x) => PerformViaPool(state, iterationSemaphore, resultSemaphore, resultAction));

            iterationSemaphore.Release();
        }

        private void InsertElement(List<int> list, EventWaitHandle allowReadEvent, EventWaitHandle allowWriteEvent)
        {
            for (int i = 0; i < 10; i++)
            {
                allowWriteEvent.WaitOne();
                list.Add(i);
                allowReadEvent.Set();
            }
        }

        private void CheckCollection(List<int> list, EventWaitHandle allowReadEvent, EventWaitHandle allowWriteEvent, Action<string> action)
        {
            for (int i = 0; i < 10; i++)
            {
                allowReadEvent.WaitOne();
                action(string.Join(", ", list));
                allowWriteEvent.Set();
            }
        }
    }
}
