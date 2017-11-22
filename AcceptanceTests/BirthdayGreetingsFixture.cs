using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using nDumbster.Smtp;

namespace AcceptanceTests
{
    public class BirthdayGreetingsFixture
    {
        public BirthdayGreetingsFixture(string inputLine)
        {
            using (SimpleSmtpServer smtp = SimpleSmtpServer.Start(8025))
            {
                using (FileStream file = File.Open("employees.txt", FileMode.Create))
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine("last_name, first_name, date_of_birth, email");
                    writer.WriteLine(inputLine);
                }

                Process service = new Process();
                service.StartInfo.Arguments = "127.0.0.1 8025 employees.txt";
#if DEBUG
                service.StartInfo.FileName = @"..\..\..\BirthdayGreetings\bin\Debug\BirthdayGreetings.exe";
#else
                service.StartInfo.FileName = @"..\..\..\BirthdayGreetings\bin\Release\BirthdayGreetings.exe";
#endif
                service.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

                try
                {
                    service.Start();
                    service.WaitForExit();
                }
                finally
                {
                    File.Delete("employees.txt");
                }

                ExitCode = service.ExitCode;
                ReceivedEmails = smtp.ReceivedEmail.ToArray();
            }
        }

        public int ExitCode { get; }
        public IReadOnlyCollection<MailMessage> ReceivedEmails { get; }
    }
}