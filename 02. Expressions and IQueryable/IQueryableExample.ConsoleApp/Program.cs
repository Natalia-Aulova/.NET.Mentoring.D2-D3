using System;
using System.Linq;
using IQueryableExample.ConsoleApp.Services;
using IQueryableExample.ConsoleApp.Services.E3SClient.Entities;

namespace IQueryableExample.ConsoleApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var employees = new E3SEntitySet<EmployeeEntity>(new SettingsProvider());

            foreach (var emp in employees.Where(e => e.WorkStation == "EPBYMINW6641"))
            {
                Console.WriteLine("{0} {1}", emp.NativeName, emp.StartWorkDate);
            }
        }
    }
}
