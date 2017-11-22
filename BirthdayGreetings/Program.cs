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

            DateTime now = DateTime.Now;

            IEmployees employees = new EmployeesFile(dataPath);

            using (var emailClient = new EmailClient(hostname, port))
                foreach (Employee employee in employees)
                    if (employee.Birthday.Month == now.Month && employee.Birthday.Day == now.Day)
                        emailClient.Send(new GreetingMessage(employee));
        }
    }
}
