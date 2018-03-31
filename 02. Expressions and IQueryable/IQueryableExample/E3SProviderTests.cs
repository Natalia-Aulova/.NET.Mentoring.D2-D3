using System;
using System.Linq;
using IQueryableExample.Services;
using IQueryableExample.Services.E3SClient;
using IQueryableExample.Services.E3SClient.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQueryableExample
{
    [TestClass]
    public class E3SProviderTests
    {
        [TestMethod]
        public void WithoutProvider_Generic()
        {
            var settingsProvider = new SettingsProvider();
            var client = new E3SQueryClient(settingsProvider.GetSetting("user"), settingsProvider.GetSetting("password"), settingsProvider.GetSetting("apiUrl"));
            var res = client.SearchFTS<EmployeeEntity>("workStation:(EPBYMINW6641)", 0, 1);

            foreach (var emp in res)
            {
                Console.WriteLine("{0} {1}", emp.NativeName, emp.StartWorkDate);
            }
        }

        [TestMethod]
        public void WithoutProvider_NonGeneric()
        {
            var settingsProvider = new SettingsProvider();
            var client = new E3SQueryClient(settingsProvider.GetSetting("user"), settingsProvider.GetSetting("password"), settingsProvider.GetSetting("apiUrl"));
            var res = client.SearchFTS(typeof(EmployeeEntity), "workStation:(EPBYMINW6641)", 0, 10);

            foreach (var emp in res.OfType<EmployeeEntity>())
            {
                Console.WriteLine("{0} {1}", emp.NativeName, emp.StartWorkDate);
            }
        }

        [TestMethod]
        public void WithProvider_Equal_DirectOrder()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(new SettingsProvider());
            
            foreach (var emp in employees.Where(e => e.WorkStation == "EPBYMINW6641"))
            {
                Console.WriteLine("{0} {1}", emp.NativeName, emp.StartWorkDate);
            }
        }

        [TestMethod]
        public void WithProvider_Equal_ReverseOrder()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(new SettingsProvider());

            foreach (var emp in employees.Where(e => "EPBYMINW6641" == e.WorkStation))
            {
                Console.WriteLine("{0} {1}", emp.NativeName, emp.StartWorkDate);
            }
        }

        [TestMethod]
        public void WithProvider_StartsWith()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(new SettingsProvider());

            foreach (var emp in employees.Where(e => e.WorkStation.StartsWith("EPBYMINW664")))
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}: {emp.WorkStation}{Environment.NewLine}");
            }
        }

        [TestMethod]
        public void WithProvider_EndsWith()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(new SettingsProvider());

            foreach (var emp in employees.Where(e => e.WorkStation.EndsWith("641")))
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}: {emp.WorkStation}{Environment.NewLine}");
            }
        }

        [TestMethod]
        public void WithProvider_Contains()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(new SettingsProvider());

            foreach (var emp in employees.Where(e => e.WorkStation.Contains("64")))
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}: {emp.WorkStation}{Environment.NewLine}");
            }
        }

        [TestMethod]
        public void WithProvider_AndAlso()
        {
            var employees = new E3SEntitySet<EmployeeEntity>(new SettingsProvider());

            foreach (var emp in employees.Where(e => e.WorkStation.StartsWith("EPBYMINW664") && e.FirstName == "Natalia"))
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}: {emp.WorkStation}{Environment.NewLine}");
            }
        }
    }
}
