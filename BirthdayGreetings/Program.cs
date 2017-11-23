using System;

namespace BirthdayGreetings
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Configuration(args);

            IEmployees employees = new EmployeesFile(config.EmployeesFilePath);

            using (var emailClient = new EmailClient(config.SmtpHostname, config.SmtpPort))
                new BirthdayGreetingsService(DateTime.Today, employees, emailClient)
                    .SendGreetings();
        }
    }
}
