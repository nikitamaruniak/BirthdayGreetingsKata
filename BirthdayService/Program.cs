using System;

namespace BirthdayService
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateService(ParseCommandLine(args)).Run();
        }

        public static Options ParseCommandLine(string[] args)
        {
            if (args.Length != 3)
            {
                throw new FormatException();
            }

            return new Options
            {
                SmtpServerHost = args[0],
                SmtpServerPort = ushort.Parse(args[1]),
                DataFilePath = args[2],
            };
        }

        public static BirthdayService CreateService(Options options)
        {
            return new BirthdayService(
                new FileEmployeeRepository(options.DataFilePath),
                new MessageBuilder(BirthdayService.FROM_EMAIL_ADDRESS),
                new SMTPMessageSender(options.SmtpServerHost, options.SmtpServerPort)
            );
        }
    }
}
