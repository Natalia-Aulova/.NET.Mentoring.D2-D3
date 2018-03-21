namespace Multithreading.Console
{
    using System;

    public class Program
    {
        private static void Main(string[] args)
        {
            var taskService = new IterationService();
            taskService.PerformWithTasks(Console.WriteLine);
        }
    }
}
