using System;
using System.Threading.Tasks;

namespace Multithreading
{
    public class IterationTaskBuilder
    {
        public Task Build(Action<string> resultAction)
        {
            return new Task(() => Perform(resultAction));
        }

        private void Perform(Action<string> action)
        {
            for (int i = 1; i <= 1000; i++)
            {
                action($"Task #{Task.CurrentId} - {i}");
            }
        }
    }
}
