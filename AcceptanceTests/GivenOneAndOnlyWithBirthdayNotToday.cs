using System;
using Xunit;

namespace AcceptanceTests
{
    public class GivenOneAndOnlyWithBirthdayNotToday : IClassFixture<GivenOneAndOnlyWithBirthdayNotToday.Fixture>
    {
        public class Fixture : MthBirthdayGreetingsFixture
        {
            public Fixture() : base(employeeLine)
            {
            }
        }

        private static readonly string employeeLine =
            string.Format("{0}, {1}, {2:yyyy\\/MM\\/dd}, {3}",
                "Doe", "John", DateTime.Now.AddDays(1),
                "john.doe@foobar.com");

        public GivenOneAndOnlyWithBirthdayNotToday(Fixture fixture)
        {
            this.fixture = fixture;
        }

        private readonly Fixture fixture;

        [Fact]
        public void ProgramReportsSuccess() =>
            Assert.Equal(0, fixture.ExitCode);

        [Fact]
        public void SendsNoEmails() =>
            Assert.Equal(0, fixture.ReceivedEmails.Count);
    }
}
