using System;
using System.Collections.Generic;

namespace BirthdayGreetings
{
    public class BirthdayGreetingsService
    {
        public BirthdayGreetingsService(
            DateTime today,
            IEmployees employees,
            ISendGreetingMessage deliveryService)
        {
            this.today = today;
            this.employees = employees;
            this.deliveryService = deliveryService;
        }

        private readonly DateTime today;
        private readonly IEmployees employees;
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
