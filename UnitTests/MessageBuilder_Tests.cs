using System;
using System.Net.Mail;
using Ploeh.AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BirthdayService.UnitTests
{
    [TestClass]
    public class MessageBuilder_Tests
    {
        private const string FROM_EMAIL_ADDRESS = "sender@example.com";
        private MessageBuilder sut;

        private IFixture fixture;

        [TestInitialize]
        public void Initialize()
        {
            fixture = new Fixture();

            fixture.Customize<Employee>(cb => cb.FromFactory<string, string, DateTime>(
                (string firstName, string lastName, DateTime birthday) => new Employee(firstName, lastName, birthday, "employee@example.com")));

            sut = new MessageBuilder(FROM_EMAIL_ADDRESS);
        }

        [TestMethod]
        public void Build_should_return_message_with_correct_from_value()
        {
            Employee employee = fixture.Create<Employee>();

            MailMessage message = sut.Build(employee);

            Assert.AreEqual(FROM_EMAIL_ADDRESS, message.From.Address);
        }

        [TestMethod]
        public void Build_should_return_message_with_correct_to_subject_and_body_values()
        {
            Employee employee = fixture.Create<Employee>();

            MailMessage actual = sut.Build(employee);

            string expectedBody = string.Format("Happy birthday, dear {0}!", employee.FirstName);
            string expectedSubject = "Happy birthday!";

            Assert.AreEqual(1, actual.To.Count, "Message.To.Count");
            Assert.AreEqual(employee.Email, actual.To[0].Address, "Message.To");
            Assert.AreEqual(expectedSubject, actual.Subject, "Message.Subject");
            Assert.AreEqual(expectedBody, actual.Body, "Message.Body");
        }

        [TestMethod]
        public void Constructor_should_set_from()
        {
            Assert.AreEqual(FROM_EMAIL_ADDRESS, sut.From);
        }
    }
}
