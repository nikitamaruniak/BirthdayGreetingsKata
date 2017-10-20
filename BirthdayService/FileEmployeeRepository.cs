using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace BirthdayService
{
    public class FileEmployeeRepository : IEmployeeRepository
    {
        private const string FIELD_SEPARATOR = ", ";
        private string filePath;

        public FileEmployeeRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }
        }

        public IEnumerable<Employee> FindByBirthday(int month, int day)
        {
            var employees = new List<Employee>();
            ForEachLineInFile((line, lineNumber) =>
                {
                    if (lineNumber == 1)
                    {
                        return;
                    }
                    Employee employee;
                    if (TryParseEmployee(line, out employee))
                    {
                        if (employee.Birthday.Month == month && employee.Birthday.Day == day)
                        {
                            employees.Add(employee);
                        }
                    }
                });

            return employees;
        }

        private void ForEachLineInFile(Action<string, int> action)
        {
            using (TextReader reader = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
            {
                int lineNumber = 1;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    action(line, lineNumber);
                    lineNumber += 1;
                }
            }
        }

        private bool TryParseEmployee(string line, out Employee employee)
        {
            string[] fields = line.Split(new string[] { FIELD_SEPARATOR }, StringSplitOptions.None);

            bool result = false;

            string firstName = string.Empty;
            string lastName = string.Empty;
            DateTime birthday = new DateTime();
            string email = string.Empty;

            if (fields.Length == 4)
            {
                lastName = fields[0];
                firstName = fields[1];
                
                if (DateTime.TryParseExact(fields[2],
                                           "yyyy/MM/dd",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out birthday))
                {
                    result = true;
                }

                email = fields[3];
            }

            employee = new Employee(firstName, lastName, birthday, email);
            return result;
        }
    }
}
