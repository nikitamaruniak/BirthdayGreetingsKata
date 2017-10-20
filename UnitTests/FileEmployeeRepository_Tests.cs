using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BirthdayService.UnitTests
{
    [TestClass]
    public class FileEmployeeRepository_Tests
    {
        private const string DATA_FILE_PATH = "TestData.txt";
        private FileEmployeeRepository sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = new FileEmployeeRepository(DATA_FILE_PATH);
            PrepareDataFile();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteDataFile();
        }

        [TestMethod]
        public void FindByBirthday_should_return_employee_that_has_birthday_today()
        {
            DateTime today = DateTime.Today;

            Employee[] expected = new []
            {
                new Employee("Anony", "Mous", today, "anonymous@example.com"),
            };

            AddEmployeesToDataFile(expected);

            Employee[] actual = sut.FindByBirthday(month: today.Month, day: today.Day).ToArray();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void FindByBirthday_should_return_employees_that_have_birthday_today()
        {
            DateTime today = DateTime.Today;

            Employee[] testData = new[]
            {
                new Employee("Anony", "MousOne", today, "anonymousone@example.com"),
                new Employee("Anony", "MousTwo", today, "anonymoustwo@example.com"),
                new Employee("Anony", "MousThree", today, "anonymousthree@example.com"),
            };

            AddEmployeesToDataFile(testData);

            Employee[] expected = testData.Where(employee => employee.Birthday.Month == today.Month
                                                          && employee.Birthday.Day == today.Day).ToArray();
            Employee[] actual = sut.FindByBirthday(today.Month, today.Day).ToArray();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void FindByBirthday_should_not_return_employees_that_dont_have_birthday_today()
        {
            DateTime today = DateTime.Today;

            Employee[] testData = new[]
            {
                new Employee("Anony", "MousOne", today.AddDays(-10), "anonymousone@example.com"),
                new Employee("Anony", "MousTwo", today, "anonymoustwo@example.com"),
                new Employee("Anony", "MousThree", today.AddDays(-20), "anonymousthree@example.com"),
            };

            AddEmployeesToDataFile(testData);

            Employee[] expected = testData.Where(employee => employee.Birthday.Month == today.Month
                                                          && employee.Birthday.Day == today.Day).ToArray();
            Employee[] actual = sut.FindByBirthday(today.Month, today.Day).ToArray();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void Constructor_should_set_filepath()
        {
            Assert.AreEqual(DATA_FILE_PATH, sut.FilePath);
        }

        private void PrepareDataFile()
        {
            using (TextWriter writer = new StreamWriter(new FileStream(DATA_FILE_PATH, FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine("last_name, first_name, date_of_birth, email");
            }
        }

        private void DeleteDataFile()
        {
            if (File.Exists(DATA_FILE_PATH))
            {
                File.Delete(DATA_FILE_PATH);
            }
        }

        private void AddEmployeesToDataFile(IEnumerable<Employee> employees)
        {
            using (TextWriter writer = new StreamWriter(new FileStream(DATA_FILE_PATH, FileMode.Append, FileAccess.Write)))
            {
                foreach (Employee employee in employees)
                {
                    writer.WriteLine("{0}, {1}, {2}, {3}",
                        employee.LastName,
                        employee.FirstName,
                        employee.Birthday.ToString("yyyy\\/MM\\/dd"),
                        employee.Email);
                }
            }
        }
    }
}
