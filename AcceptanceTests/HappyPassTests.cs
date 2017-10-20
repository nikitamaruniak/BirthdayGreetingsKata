using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using nDumbster.Smtp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BirthdayService.AcceptanceTests
{
    [TestClass]
    public class HappyPassTests
    {
        private const string SERVICE_FILE_NAME =
            @"..\..\..\BirthdayService\Bin\Debug\BirthdayService.exe";
        private const string DATA_FILE_NAME = "Data.txt";
        private const int SMTP_PORT = 8025;

        private SimpleSmtpServer _smtpServer;
        
        [TestInitialize]
        public void Initialize()
        {
            _smtpServer = SimpleSmtpServer.Start(SMTP_PORT);
            PrepareDataFileWithHeader();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _smtpServer.Stop();
            DeleteDataFile();
        }

        [TestMethod]
        public void Person_who_has_birthday_today_receives_message()
        {
            AppendRecordToDataFile("Garry", "Bob", DateTime.Today, "bob.garry@example.com");

            int serviceExitCode = ExecuteService();

            Assert.AreEqual(0, serviceExitCode, "service exit code");
            
            Assert.AreEqual(1, _smtpServer.ReceivedEmailCount, "number of messages");
            
            MailMessage message = _smtpServer.ReceivedEmail.First();

            Assert.AreEqual(1, message.To.Count, "number of recipients");
            Assert.AreEqual("bob.garry@example.com", message.To.First().Address, "recipient");
            Assert.AreEqual("Happy birthday!", message.Subject, "subject");
            Assert.AreEqual("Happy birthday, dear Bob!", message.Body, "message body");
        }

        private int ExecuteService()
        {
            string arguments = string.Format("127.0.0.1 {0} {1}", SMTP_PORT, DATA_FILE_NAME);
            Process service = Process.Start(SERVICE_FILE_NAME, arguments);
            service.WaitForExit();
            return service.ExitCode;
        }

        private void PrepareDataFileWithHeader()
        {
            using (TextWriter writer = new StreamWriter(new FileStream(DATA_FILE_NAME, FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine("last_name, first_name, date_of_birth, email");
            }
        }

        private void AppendRecordToDataFile(string lastName, string firstName, DateTime birthday, string email)
        {
            using (TextWriter writer = new StreamWriter(new FileStream(DATA_FILE_NAME, FileMode.Append, FileAccess.Write)))
            {
                writer.WriteLine("{0}, {1}, {2}, {3}", lastName, firstName, birthday.ToString("yyyy\\/MM\\/dd"), email);
            }
        }

        private void DeleteDataFile()
        {
            if (File.Exists(DATA_FILE_NAME))
            {
                File.Delete(DATA_FILE_NAME);
            }
        }
    }
}
