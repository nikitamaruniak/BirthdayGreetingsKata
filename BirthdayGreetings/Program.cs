using System;
using System.Net.Mail;

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

            foreach (Employee employee in employees)
            {
                if (employee.Birthday.Month == now.Month && employee.Birthday.Day == now.Day)
                {
                    using (SmtpClient smtpClient = new SmtpClient(hostname, port))
                    {
                        var msg = new GreetingMessage(employee);
                        smtpClient.Send(new MailMessage("noreply@noname.com", msg.To, msg.Subject, msg.Body));
                    }
                }
            }
        }
    }
}
