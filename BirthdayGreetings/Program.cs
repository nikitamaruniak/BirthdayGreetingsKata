using System;
using System.Collections.Generic;

namespace BirthdayGreetings
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Configuration(args);

            IEnumerable<Employee> employees =
                new EmployeesFile(config.EmployeesFilePath);

            using (var emailClient = new EmailClient(config.SmtpHostname, config.SmtpPort))
                new BirthdayGreetingsService(DateTime.Today, employees, emailClient)
                    .SendGreetings();
        }
    }
}
