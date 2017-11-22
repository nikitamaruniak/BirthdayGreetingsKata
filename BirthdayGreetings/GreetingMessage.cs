namespace BirthdayGreetings
{
    public class GreetingMessage
    {
        public GreetingMessage(Employee employee)
        {
            To = employee.Email;
            Subject = "Happy birthday!";
            Body = $"Happy birthday, dear {employee.FirstName}!";
        }

        public string To { get; }

        public string Subject { get; }

        public string Body { get; }
    }
}