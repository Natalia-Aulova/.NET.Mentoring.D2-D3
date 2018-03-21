using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading.Console
{
    public class IterationService
    {
        private readonly int _workersCount = 100;
        private readonly int _iterationsCount = 1000;

        private readonly object _obj = new object();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public void PerformWithTasks(Action<string> resultAction)
        {
            var tasks = new Task[_workersCount];

            for (int i = 0; i < _workersCount; i++)
            {
                var task = new Task(() => TaskBody(resultAction));
                task.Start();
                tasks[i] = task;
            }

            Task.WaitAll(tasks);
        }

        public void PerformWithThreads(Action<string> resultAction)
        {
            var threads = new List<Thread>();

            for (int i = 0; i < _workersCount; i++)
            {
                var thread = new Thread(() => ThreadBody(resultAction));
                thread.Start();
                threads.Add(thread);
            }

            threads.ForEach(x => x.Join());
        }

        private void TaskBody(Action<string> action)
        {
            for (int i = 0; i < _iterationsCount; i++)
            {
                lock (_obj)
                {
                    action($"Task #{Task.CurrentId} - {i}");
                }
            }
        }

        private void ThreadBody(Action<string> action)
        {
            for (int i = 0; i < _iterationsCount; i++)
            {
                _semaphore.Wait();
                action($"Thread #{Thread.CurrentThread.ManagedThreadId} - {i}");
                _semaphore.Release();
            }
        }
    }
}
