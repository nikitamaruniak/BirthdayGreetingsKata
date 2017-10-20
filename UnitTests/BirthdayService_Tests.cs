using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using NSubstitute;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace BirthdayService.UnitTests
{
    [TestClass]
    public class BirthdayService_Tests
    {
        private IFixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [TestMethod]
        public void Run_should_call_repository_with_today_month_and_day()
        {
            IEmployeeRepository mockRepository = fixture.Freeze<IEmployeeRepository>();
            var sut = fixture.Create<BirthdayService>();

            DateTime today = DateTime.Today;
            int expectedMonth = today.Month;
            int expectedDay = today.Day;

            sut.Run();

            mockRepository.Received().FindByBirthday(expectedMonth, expectedDay);
        }

        [TestMethod]
        public void Run_should_call_messagebuilder_for_every_employee_returned_from_repository()
        {
            IEmployeeRepository stubRepository = fixture.Freeze<IEmployeeRepository>();
            IMessageBuilder spyMessageBuilder = fixture.Freeze<IMessageBuilder>();
            var sut = fixture.Create<BirthdayService>();

            DateTime today = DateTime.Today;

            IEnumerable<Employee> testEmployees = new[]
            {
                new Employee("Anony", "MousOne", today, "anonymousone@example.com"),
                new Employee("Anony", "MousTwo", today, "anonymoustwo@example.com"),
                new Employee("Anony", "MousThree", today, "anonymousthree@example.com"),
            };

            stubRepository.FindByBirthday(Arg.Any<int>(), Arg.Any<int>()).Returns(testEmployees);

            var capturedArgs = new List<Employee>();

            //spyMessageBuilder.Build(Arg.Do<Employee>(e => capturedArgs.Add(e)));
            spyMessageBuilder
                .When(builder => builder.Build(Arg.Any<Employee>()))
                .Do(call => capturedArgs.Add((Employee)call.Args()[0]));

            sut.Run();

            Employee[] expected = testEmployees.ToArray();
            Employee[] actual = capturedArgs.ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Run_should_send_messages_to_all_employees_returned_from_repository()
        {
            IEmployeeRepository stubRepository = fixture.Freeze<IEmployeeRepository>();
            IMessageBuilder stubMessageBuilder = fixture.Freeze<IMessageBuilder>();
            IMessageSender spyMessageSender = fixture.Freeze<IMessageSender>();
            var sut = fixture.Create<BirthdayService>();

            DateTime today = DateTime.Today;
            IEnumerable<Employee> testEmployees = new[]
            {
                new Employee("Anony", "MousOne", today, "anonymousone@example.com"),
                new Employee("Anony", "MousTwo", today, "anonymoustwo@example.com"),
                new Employee("Anony", "MousThree", today, "anonymousthree@example.com"),
            };

            stubRepository.FindByBirthday(Arg.Any<int>(), Arg.Any<int>()).Returns(testEmployees);
            stubMessageBuilder
                .Build(Arg.Any<Employee>())
                .Returns(call => new MailMessage("from@example.com", ((Employee)call.Args()[0]).Email));

            var capturedArgs = new List<MailMessage>();
            spyMessageSender
                .When(sender => sender.Send(Arg.Any<MailMessage>()))
                .Do(call => capturedArgs.Add((MailMessage)call.Args()[0]));

            sut.Run();

            string[] expected = testEmployees.Select(e => e.Email).ToArray();
            string[] actual = capturedArgs.Select(message => message.To[0].Address).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor_should_set_repository()
        {
            var repository = fixture.Freeze<IEmployeeRepository>();
            var sut = fixture.Create<BirthdayService>();

            IEmployeeRepository expected = repository;
            IEmployeeRepository actual = sut.Repository;

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void Constructor_should_set_messagebuilder()
        {
            var messageBuilder = fixture.Freeze<IMessageBuilder>();
            var sut = fixture.Create<BirthdayService>();

            IMessageBuilder expected = messageBuilder;
            IMessageBuilder actual = sut.MessageBuilder;

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void Constructor_should_set_messagesender()
        {
            var messageBuilder = fixture.Freeze<IMessageSender>();
            var sut = fixture.Create<BirthdayService>();

            IMessageSender expected = messageBuilder;
            IMessageSender actual = sut.MessageSender;

            Assert.AreSame(expected, actual);
        }
    }
}
