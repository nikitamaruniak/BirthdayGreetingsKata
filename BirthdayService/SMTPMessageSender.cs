using System.Net.Mail;

namespace BirthdayService
{
    public class SMTPMessageSender : IMessageSender
    {
        public string ServerHost { get; private set; }
        
        public ushort ServerPort { get; private set; }

        public SMTPMessageSender(string serverHost, ushort serverPort)
        {
            ServerHost = serverHost;
            ServerPort = serverPort;
        }

        public void Send(MailMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient(ServerHost, ServerPort))
            {
                smtpClient.Send(message);
            }
        }
    }
}
