using System;
using System.Linq;
using Xunit;

namespace AcceptanceTests
{
    public class GivenOneAndOnlyBirthdayToday : IClassFixture<GivenOneAndOnlyBirthdayToday.Fixture>
    {
        public class Fixture : MthBirthdayGreetingsFixture
        {
            public Fixture() : base(employeeLine)
            {   
            }
        }

        private static readonly string employeeLine =
            string.Format("{0}, {1}, {2:yyyy\\/MM\\/dd}, {3}",
                "Doe", "John", DateTime.Now,
                "john.doe@foobar.com");

        public GivenOneAndOnlyBirthdayToday(Fixture fixture)
        {
            this.fixture = fixture;
        }

        private readonly Fixture fixture;

        [Fact]
        public void ProgramReportsSuccess() =>
            Assert.Equal(0, fixture.ExitCode);

        [Fact]
        public void SendsOneEmail() =>
            Assert.Equal(1, fixture.ReceivedEmails.Count);

        [Fact]
        public void EmailHasCorrectToAddress() =>
            Assert.Equal(
                new[] {"john.doe@foobar.com"},
                fixture.ReceivedEmails.First().To.Select(to => to.Address).ToArray());

        [Fact]
        public void EmailHasCorrectSubject() =>
            Assert.Equal("Happy birthday!", fixture.ReceivedEmails.First().Subject);

        [Fact]
        public void EmailHasCorrectBody() =>
            Assert.Equal("Happy birthday, dear John!", fixture.ReceivedEmails.First().Body);
    }
}
