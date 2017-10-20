using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace BirthdayService.UnitTests
{
    [TestClass]
    public class Program_Tests
    {
        private Fixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            fixture = new Fixture();
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseCommandLine_should_throw_formatexception_if_command_line_is_empty()
        {
            Program.ParseCommandLine(new string[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseCommandLine_should_throw_formatexception_if_command_line_contains_less_than_three_parameters()
        {
            Program.ParseCommandLine(fixture.CreateMany<string>(2).ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseCommandLine_should_throw_formatexception_if_command_line_contains_more_than_three_parameters()
        {
            Program.ParseCommandLine(fixture.CreateMany<string>(4).ToArray());
        }

        [TestMethod]
        public void ParseCommandLine_should_parse_first_parameter_as_smtp_server_host()
        {
            string[] testParameters = new[]
            {
                "127.0.0.1",
                "8025",
                @".\Employees.txt",
            };

            string expected = testParameters[0];
            string actual = Program.ParseCommandLine(testParameters).SmtpServerHost;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCommandLine_should_parse_second_parameter_as_smtp_server_port()
        {
            string[] testParameters = new[]
            {
                "127.0.0.1",
                "8025",
                @".\Employees.txt",
            };

            ushort expected = ushort.Parse(testParameters[1]);
            ushort actual = Program.ParseCommandLine(testParameters).SmtpServerPort;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCommandLine_should_parse_third_parameter_as_data_file_path()
        {
            string[] testParameters = new[]
            {
                "127.0.0.1",
                "8025",
                @".\Employees.txt",
            };

            string expected = testParameters[2];
            string actual = Program.ParseCommandLine(testParameters).DataFilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateService_should_return_not_null()
        {
            Options someOptions = fixture.Create<Options>();

            BirthdayService actual = Program.CreateService(options: someOptions);

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void CreateService_should_create_repository_with_path_from_options()
        {
            var expected = "Employee.txt";

            Options someOptions = new Options { DataFilePath = expected };

            BirthdayService service = Program.CreateService(someOptions);

            var actual = ((FileEmployeeRepository)service.Repository).FilePath;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateService_should_create_messagebuilder_with_birthdayservice_email_address()
        {
            Options someOptions = fixture.Create<Options>();

            BirthdayService service = Program.CreateService(someOptions);

            var actual = ((MessageBuilder)service.MessageBuilder).From;

            Assert.AreEqual(BirthdayService.FROM_EMAIL_ADDRESS, actual);
        }

        [TestMethod]
        public void CreateService_should_create_messagesender_with_host_and_port_from_options()
        {
            Options someOptions = fixture.Create<Options>();

            BirthdayService service = Program.CreateService(someOptions);

            var actualMessageSender = (SMTPMessageSender)service.MessageSender;

            Assert.AreEqual(someOptions.SmtpServerHost, actualMessageSender.ServerHost);
            Assert.AreEqual(someOptions.SmtpServerPort, actualMessageSender.ServerPort);
        }
    }
}
