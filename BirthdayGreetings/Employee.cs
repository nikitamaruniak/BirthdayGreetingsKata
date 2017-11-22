using System;

namespace BirthdayGreetings
{
    public class Employee
    {
        public Employee(string firstName, DateTime birthday, string email)
        {
            FirstName = firstName;
            Birthday = birthday;
            Email = email;
        }

        public string FirstName { get; }
        public DateTime Birthday { get; }
        public string Email { get; }
    }
}
