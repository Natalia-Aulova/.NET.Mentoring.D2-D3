using System;
using System.Threading.Tasks;

namespace Multithreading.UI
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //TaskIterations();
            //TaskChain();
            //MultiplyMatrices();
            RecursiveThreads();
            //RecursiveThreadPool();
            //SharedCollection();
        }

        private static void TaskIterations()
        {
            var taskCount = 100;
            var tasks = new Task[taskCount];
            var taskBuilder = new IterationTaskBuilder();

            for (int i = 0; i < taskCount; i++)
            {
                var task = taskBuilder.Build(Console.WriteLine);
                task.Start();
                tasks[i] = task;
            }

            Task.WaitAll(tasks);
        }

        private static void TaskChain()
        {
            var taskBuilder = new ChainTaskService();
            
            var taskChain = taskBuilder.CreateRandomArray(Console.WriteLine)
                .ContinueWith((x) => taskBuilder.AddRandomNumber(Console.WriteLine, x.Result))
                .Unwrap()
                .ContinueWith(x => taskBuilder.SortArray(Console.WriteLine, x.Result))
                .Unwrap()
                .ContinueWith(x => taskBuilder.CalculateTheAverageValue(Console.WriteLine, x.Result))
                .Unwrap();

            Task.WaitAll(taskChain);
        }

        private static void MultiplyMatrices()
        {
            var firstMatrix = new[,]
            {
                { 5, 6, 3, 7 },
                { 4, 7, 9, 10 },
                { 4, 7, 13, 2 },
            };

            var secondMatrix = new[,]
            {
                { 5, 6, 3, 7, 8 },
                { 4, 7, 9, 10, 6 },
                { 4, 5, 13, 2, 1 },
                { 18, 2, 9, 14, 0 },
            };
            
            var matrixService = new MatrixService();
            var result = matrixService.Multiply(firstMatrix, secondMatrix);

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    Console.Write($"{result[i, j]}\t");
                }
                Console.WriteLine();
            }
        }

        private static void RecursiveThreads()
        {
            var threadService = new ThreadService();
            threadService.UseThreads(Console.WriteLine);
        }

        private static void RecursiveThreadPool()
        {
            var threadService = new ThreadService();
            threadService.UseThreadPool(Console.WriteLine);
        }

        private static void SharedCollection()
        {
            var threadService = new ThreadService();
            threadService.CreateCollection(Console.WriteLine);
        }
    }
}
