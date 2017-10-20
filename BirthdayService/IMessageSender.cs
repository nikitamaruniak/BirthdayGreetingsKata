using System.Net.Mail;

namespace BirthdayService
{
    public interface IMessageSender
    {
        void Send(MailMessage message);
    }
}
