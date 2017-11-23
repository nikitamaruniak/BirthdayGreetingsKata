using System;

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
            foreach (Employee employee in employees)
                if (employee.Birthday.Month == today.Month && employee.Birthday.Day == today.Day)
                    deliveryService.Send(new GreetingMessage(employee));
        }
    }
}
