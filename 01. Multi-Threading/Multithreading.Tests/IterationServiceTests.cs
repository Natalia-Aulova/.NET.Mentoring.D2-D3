using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Multithreading.Console;
using NUnit.Framework;

namespace Multithreading.Tests
{
    [TestFixture]
    public class IterationServiceTests
    {
        private readonly int _workersCount = 100;
        private readonly int _iterationsCount = 1000;
        private readonly string _taskRegexPattern = @"Task #(\d+) - (\d+)";
        private readonly string _threadRegexPattern = @"Thread #(\d+) - (\d+)";

        [Test]
        public void PerformWithTasks_ShouldPerformTheCorrectNumberOfIterations()
        {
            var builder = new StringBuilder();
            var taskService = new IterationService();

            taskService.PerformWithTasks(x => builder.Append($"{x}"));
            var result = builder.ToString();

            var actual = new Regex(_taskRegexPattern).Matches(result);
            var matchedGroups = actual.Cast<Match>().GroupBy(x => x.Groups[1].Value).ToList();

            Assert.AreEqual(_workersCount * _iterationsCount, actual.Count);
            Assert.AreEqual(_workersCount, matchedGroups.Count);

            matchedGroups.ForEach(x =>
            {
                Assert.AreEqual(_iterationsCount, x.Count());
            });
        }

        [Test]
        public void PerformWithThreads_ShouldPerformTheCorrectNumberOfIterations()
        {
            var builder = new StringBuilder();
            var taskService = new IterationService();

            taskService.PerformWithThreads(x => builder.Append($"{x}"));
            var result = builder.ToString();

            var actual = new Regex(_threadRegexPattern).Matches(result);
            var matchedGroups = actual.Cast<Match>().GroupBy(x => x.Groups[1].Value).ToList();

            Assert.AreEqual(_workersCount * _iterationsCount, actual.Count);
            Assert.AreEqual(_workersCount, matchedGroups.Count);

            matchedGroups.ForEach(x =>
            {
                Assert.AreEqual(_iterationsCount, x.Count());
            });
        }
    }
}
