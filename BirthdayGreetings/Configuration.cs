namespace BirthdayGreetings
{
    public class Configuration
    {
        public Configuration(string[] commandLineArgs)
        {
            SmtpHostname = commandLineArgs[0];
            SmtpPort = int.Parse(commandLineArgs[1]);
            EmployeesFilePath = commandLineArgs[2];
        }

        public string SmtpHostname { get; }

        public int SmtpPort { get; }

        public string EmployeesFilePath { get; }
    }
}
