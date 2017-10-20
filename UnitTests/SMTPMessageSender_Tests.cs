using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using nDumbster.Smtp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BirthdayService.UnitTests
{
    [TestClass]
    public class SMTPMessageSender_Tests
    {
        private const ushort SMTP_PORT = 8025;

        SimpleSmtpServer smtpServer;

        [TestInitialize]
        public void Initialize()
        {
            smtpServer = SimpleSmtpServer.Start(SMTP_PORT);
        }

        [TestCleanup]
        public void Cleanup()
        {
            smtpServer.Stop();
        }

        [TestMethod]
        public void Constructor_sets_host_and_port()
        {
            SMTPMessageSender sut = new SMTPMessageSender("127.0.0.1", SMTP_PORT);

            Assert.AreEqual("127.0.0.1", sut.ServerHost);
            Assert.AreEqual(SMTP_PORT, sut.ServerPort);
        }

        [TestMethod]
        public void Send_should_close_tcp_connection_in_the_end()
        {
            MailMessage message = new MailMessage(
                "from@bar.com",
                "to@bar.com",
                "Test Subject",
                "Test Message Body");

            SMTPMessageSender sut = new SMTPMessageSender("127.0.0.1", SMTP_PORT);

            sut.Send(message);

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();

            var smtpEndPoint = "127.0.0.1:" + SMTP_PORT;

            bool connectionExist = connections.Any(info => info.RemoteEndPoint.ToString() == smtpEndPoint
                                                        && info.State == TcpState.Established);

            Assert.IsFalse(connectionExist, "Opened connection exists.");
        }

        [TestMethod]
        public void Send_sends_message_through_SMTP_server()
        {
            MailMessage message = new MailMessage(
                "from@bar.com",
                "to@bar.com",
                "Test Subject",
                "Test Message Body");

            SMTPMessageSender sut = new SMTPMessageSender("127.0.0.1", SMTP_PORT);

            sut.Send(message);
            
            Assert.AreEqual(1, smtpServer.ReceivedEmailCount, "received count");

            MailMessage actual = smtpServer.ReceivedEmail.First();

            Assert.AreEqual(1, actual.To.Count, "number of recipients");
            Assert.AreEqual(message.To.First().Address, actual.To.First().Address, "recipient");
            Assert.AreEqual(message.Subject, actual.Subject, "subject");
            Assert.AreEqual(message.Body, actual.Body, "message body");
        }
    }
}
