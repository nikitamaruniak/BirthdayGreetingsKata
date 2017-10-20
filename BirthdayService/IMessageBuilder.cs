using System.Net.Mail;

namespace BirthdayService
{
    public interface IMessageBuilder
    {
        MailMessage Build(Employee employee);
    }
}
