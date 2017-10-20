using System.Net.Mail;

namespace BirthdayService
{
    public class MessageBuilder : IMessageBuilder
    {
        private string from;

        public MessageBuilder(string from)
        {
            this.from = from;
        }

        public string From
        {
            get
            {
                return from;
            }
        }

        public MailMessage Build(Employee employee)
        {
            string subject = "Happy birthday!";
            string body = string.Format("Happy birthday, dear {0}!", employee.FirstName);
            return new MailMessage(from, employee.Email, subject, body);
        }
    }
}
