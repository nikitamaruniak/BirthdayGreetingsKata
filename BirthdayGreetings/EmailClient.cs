using System;
using System.Net.Mail;

namespace BirthdayGreetings
{
    public sealed class EmailClient : ISendGreetingMessage, IDisposable
    {
        public EmailClient(string hostname, int port)
        {
            smtpClient = new SmtpClient(hostname, port);
        }

        private readonly SmtpClient smtpClient;

        public void Send(GreetingMessage message)
        {
            var mailMessage =
                new MailMessage(
                    "noreply@noname.com",
                    message.To,
                    message.Subject,
                    message.Body);
            smtpClient.Send(mailMessage);
        }

        public void Dispose()
        {
            smtpClient.Dispose();
        }
    }
}