using System;

namespace BirthdayService
{
    public class Employee
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Email { get; private set; }

        public Employee(string firstName, string lastName, DateTime birthday, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            Email = email;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;
            
            Employee another = obj as Employee;
            if (another == null)
                return false;

            return FirstName == another.FirstName
                && LastName == another.LastName
                && Birthday == another.Birthday
                && Email == another.Email;
        }

        public override int GetHashCode()
        {
            int result = FirstName.GetHashCode();
            result = (result * 397) ^ LastName.GetHashCode();
            result = (result * 397) ^ Birthday.GetHashCode();
            result = (result * 397) ^ Email.GetHashCode();
            return result;
        }
    }
}
