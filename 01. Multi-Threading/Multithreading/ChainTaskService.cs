using System;
using System.Linq;
using System.Threading.Tasks;

namespace Multithreading
{
    public class ChainTaskService
    {
        private readonly Random _random;

        public ChainTaskService()
        {
            _random = new Random();
        }

        public Task<int[]> CreateRandomArray(Action<string> resultAction)
        {
            return Task.Factory.StartNew(() => Perform(resultAction));
        }

        public Task<int[]> AddRandomNumber(Action<string> resultAction, int[] currentArray)
        {
            return Task.Factory.StartNew(() => Perform(resultAction, currentArray));
        }

        public Task<int[]> SortArray(Action<string> resultAction, int[] array)
        {
            return Task.Factory.StartNew(() =>
            {
                Array.Sort(array);
                resultAction(string.Join(", ", array));
                return array;
            });
        }

        public Task<double> CalculateTheAverageValue(Action<string> resultAction, int[] array)
        {
            return Task.Factory.StartNew(() =>
            {
                var average = (double)array.Sum() / array.Length;
                resultAction($"{average:#.###}");
                return average;
            });
        }

        private int[] Perform(Action<string> action)
        {
            var count = 10;
            var array = new int[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = _random.Next(100);
            }

            action(string.Join(", ", array));

            return array;
        }

        private int[] Perform(Action<string> action, int[] currentArray)
        {
            var array = new int[currentArray.Length + 1];
            currentArray.CopyTo(array, 0);

            array[currentArray.Length] = _random.Next(100);

            action(string.Join(", ", array));

            return array;
        }
    }
}
