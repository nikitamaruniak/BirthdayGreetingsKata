using System;

namespace BirthdayGreetings
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostname = args[0];
            int port = int.Parse(args[1]);
            string dataPath = args[2];

            IEmployees employees = new EmployeesFile(dataPath);

            using (var emailClient = new EmailClient(hostname, port))
                new BirthdayGreetingsService(DateTime.Today, employees, emailClient)
                    .SendGreetings();
        }
    }
}
