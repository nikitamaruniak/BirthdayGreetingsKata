using System.Collections.Generic;

namespace BirthdayService
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> FindByBirthday(int month, int day);
    }
}
