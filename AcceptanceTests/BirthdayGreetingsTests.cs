using nDumbster.Smtp;
using System;
using System.Linq;
using System.IO;
using Xunit;
using System.Diagnostics;
using System.Net.Mail;

namespace AcceptanceTests
{
    public class BirthdayGreetingsTests
    {
        [Fact]
        public void GivenBirthdayTodaySendsEmail()
        {
            using (SimpleSmtpServer smtp = SimpleSmtpServer.Start(8025))
            {
                using (FileStream file = File.Open("employees.txt", FileMode.Create))
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine("last_name, first_name, date_of_birth, email");
                    writer.WriteLine(
                        "{0}, {1}, {2}, {3}",
                        "Doe", "John",
                        DateTime.Now.ToString(@"yyyy\/MM\/dd"),
                        "john.doe@foobar.com");
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

                Assert.Equal(0, service.ExitCode);
                Assert.Equal(1, smtp.ReceivedEmailCount);
                MailMessage message = smtp.ReceivedEmail.First();
                Assert.Single(message.To);
                Assert.Equal("john.doe@foobar.com", message.To.First().Address);
                Assert.Equal("Happy birthday!", message.Subject);
                Assert.Equal("Happy birthday, dear John!", message.Body);
            }
        }
    }
}
