using System;
using System.Collections.Generic;

namespace BirthdayGreetings
{
    public class BirthdayGreetingsService
    {
        public BirthdayGreetingsService(
            DateTime today,
            IEnumerable<Employee> employees,
            ISendGreetingMessage deliveryService)
        {
            this.today = today;
            this.employees = employees;
            this.deliveryService = deliveryService;
        }

        private readonly DateTime today;
        private readonly IEnumerable<Employee> employees;
        private readonly ISendGreetingMessage deliveryService;

        public void SendGreetings()
        {
            var birthdayCalendar = new BirthdayCalendar(employees);

            IEnumerable<Employee> celebratingEmployees =
                birthdayCalendar.AllCelebrating(today);

            foreach (Employee employee in celebratingEmployees)
                deliveryService.Send(new GreetingMessage(employee));
        }
    }
}
