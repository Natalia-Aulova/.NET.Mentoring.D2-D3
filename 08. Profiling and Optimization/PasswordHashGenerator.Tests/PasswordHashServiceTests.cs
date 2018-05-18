using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PasswordHashGenerator.Tests
{
    [TestClass]
    public class PasswordHashServiceTests
    {
        private readonly int _iterationCount = 50;
        private readonly string _password = "123456";
        private readonly byte[] _salt = { 12, 23, 34, 45, 56, 67, 78, 89, 90, 01, 12, 23, 34, 45, 56, 67 };

        private PasswordHashService _hashService;

        [TestInitialize]
        public void Setup()
        {
            _hashService = new PasswordHashService();
        }

        [TestMethod]
        public void Test_GeneratePasswordHashUsingSalt_Original()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < _iterationCount; i++)
            {
                _hashService.GeneratePasswordHashUsingSalt(_password, _salt);
            }

            stopwatch.Stop();

            Console.WriteLine($"original: {stopwatch.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void Test_GeneratePasswordHashUsingSalt_v2()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < _iterationCount; i++)
            {
                _hashService.GeneratePasswordHashUsingSalt_v2(_password, _salt);
            }

            stopwatch.Stop();

            Console.WriteLine($"v2: {stopwatch.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void Test_GeneratePasswordHashUsingSalt_v3()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < _iterationCount; i++)
            {
                _hashService.GeneratePasswordHashUsingSalt_v3(_password, _salt);
            }

            stopwatch.Stop();

            Console.WriteLine($"v3: {stopwatch.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void Test_GeneratePasswordHashUsingSalt_v4()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < _iterationCount; i++)
            {
                _hashService.GeneratePasswordHashUsingSalt_v4(_password, _salt);
            }

            stopwatch.Stop();

            Console.WriteLine($"v4: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
