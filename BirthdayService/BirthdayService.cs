using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace BirthdayService
{
    public class BirthdayService
    {
        public const string FROM_EMAIL_ADDRESS = "noreply@birthday.com";

        private readonly IEmployeeRepository repository;
        private readonly IMessageBuilder messageBuilder;
        private readonly IMessageSender messageSender;

        public BirthdayService(IEmployeeRepository repository, IMessageBuilder messageBuilder, IMessageSender messageSender)
        {
            this.repository = repository;
            this.messageBuilder = messageBuilder;
            this.messageSender = messageSender;
        }

        public IEmployeeRepository Repository
        {
            get
            {
                return repository;
            }
        }

        public IMessageBuilder MessageBuilder
        {
            get
            {
                return messageBuilder;
            }
        }

        public IMessageSender MessageSender
        {
            get
            {
                return messageSender;
            }
        }

        public void Run()
        {
            var today = DateTime.Today;
            IEnumerable<Employee> employees =
                repository.FindByBirthday(today.Month, today.Day);
            List<MailMessage> messages =
                employees.Select(e => messageBuilder.Build(e)).ToList();
            messages.ForEach(m => messageSender.Send(m));
        }
    }
}
