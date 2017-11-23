using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BirthdayGreetings
{
    public class EmployeesFile : IEnumerable<Employee>
    {
        public EmployeesFile(string path)
        {
            this.path = path;
        }

        private readonly string path;

        public IEnumerator<Employee> GetEnumerator()
        {
            using (FileStream file = File.Open(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(file))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] tokens = line.Split(
                        new string[] {", "},
                        StringSplitOptions.None);
                    string firstName = tokens[1];
                    var birthday =
                        DateTime.ParseExact(
                            tokens[2],
                            "yyyy/MM/dd",
                            CultureInfo.InvariantCulture);
                    string email = tokens[3];

                    yield return new Employee(firstName, birthday, email);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
