using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace AcceptanceTests
{
    public class MthBirthdayGreetingsFixture
    {
        public MthBirthdayGreetingsFixture(string employeeLine)
        {
            lock (sync)
            {
                var sthFixture =
                    new BirthdayGreetingsFixture(employeeLine);
                ExitCode = sthFixture.ExitCode;
                ReceivedEmails = sthFixture.ReceivedEmails.ToArray();
            }
        }

        private static readonly object sync = new object();

        public int ExitCode { get; }
        public IReadOnlyCollection<MailMessage> ReceivedEmails { get; }
    }
}
