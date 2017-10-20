using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BirthdayService.UnitTests
{
    [TestClass]
    public class Employee_Tests
    {
        private IFixture _fixture;

        [TestInitialize]
        public void Initialize()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [TestMethod]
        public void Constructor_sets_all_properties()
        {
            string firstName = _fixture.Create<string>();
            string lastName = _fixture.Create<string>();
            DateTime birthday = _fixture.Create<DateTime>();
            string email = "foo@bar.com";

            Employee sut = new Employee(firstName, lastName, birthday, email);

            Assert.AreEqual(firstName, sut.FirstName, "FirstName");
            Assert.AreEqual(lastName, sut.LastName, "LastName");
            Assert.AreEqual(birthday, sut.Birthday, "Birthday");
            Assert.AreEqual(email, sut.Email, "Email");
        }

        [TestMethod]
        public void Equals_should_return_true_if_all_field_are_equal()
        {
            Employee e1 = new Employee("Anon", "Mous", new DateTime(2001, 1, 1), "anonymous@example.com");
            Employee e2 = new Employee("Anon", "Mous", new DateTime(2001, 1, 1), "anonymous@example.com");

            Assert.AreEqual(true, e1.Equals(e2));
            Assert.AreEqual(true, e2.Equals(e1));
        }

        [TestMethod]
        public void Equals_should_return_false_if_some_fields_are_different()
        {
            Employee e1 = new Employee("Alice", "Anonymous", new DateTime(2001, 1, 1), "alice@example.com");
            Employee e2 = new Employee("Bob", "Anonymous", new DateTime(2001, 1, 1), "bob@example.com");

            Assert.AreEqual(false, e1.Equals(e2));
            Assert.AreEqual(false, e2.Equals(e1));
        }

        [TestMethod]
        public void GetHashCode_should_be_the_same_if_two_employees_are_equal()
        {
            Employee e1 = new Employee("Anon", "Mous", new DateTime(2001, 1, 1), "anonymous@example.com");
            Employee e2 = new Employee("Anon", "Mous", new DateTime(2001, 1, 1), "anonymous@example.com");

            bool actual = e1.GetHashCode() == e2.GetHashCode();

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void GetHashCode_should_be_not_the_same_if_two_employees_are_not_equal()
        {
            Employee e1 = new Employee("Alice", "Anonymous", new DateTime(2001, 1, 1), "alice@example.com");
            Employee e2 = new Employee("Bob", "Anonymous", new DateTime(2001, 1, 1), "bob@example.com");

            bool actual = e1.GetHashCode() == e2.GetHashCode();

            Assert.AreEqual(false, actual);
        }
    }
}
