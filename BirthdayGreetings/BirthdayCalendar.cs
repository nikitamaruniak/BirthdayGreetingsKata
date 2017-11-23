using System;
using System.Collections.Generic;
using System.Linq;

namespace BirthdayGreetings
{
    public class BirthdayCalendar
    {
        public BirthdayCalendar(IEnumerable<Employee> employees)
        {
            this.employees = employees;
        }

        private readonly IEnumerable<Employee> employees;

        public IEnumerable<Employee> AllCelebrating(DateTime onDate) =>
            employees.Where(e => e.Birthday.Month == onDate.Month && e.Birthday.Day == onDate.Day);
    }
}
