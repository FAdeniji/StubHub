using System;
using System.Collections.Generic;
using System.Linq;
namespace Viagogo
{
    public static class Location
    {
        public static int GetDistance(string fromCity, string toCity)
        {
            return AlphabeticalDistance(fromCity, toCity);
        }
        public static int AlphabeticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
    public class Event
    {
        public static readonly List<Event> AllEvents = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event { Name = "Metallica", City = "New York" },
                new Event { Name = "Metallica", City = "Boston" },
                new Event { Name = "LadyGaGa", City = "New York" },
                new Event { Name = "LadyGaGa", City = "Boston" },
                new Event { Name = "LadyGaGa", City = "Chicago" },
                new Event { Name = "LadyGaGa", City = "San Francisco" },
                new Event { Name = "LadyGaGa", City = "Washington" }
            };

        public string Name { get; set; }
        public string City { get; set; }
        public static List<Event> GetEvents() => AllEvents;
        public static List<Event> GetEventsByCity(string city) => AllEvents.Where(ae => ae.City == city).ToList();

        public static int GetPrice(Event e)
        {
            return (Location.AlphabeticalDistance(e.City, "") + Location.AlphabeticalDistance(e.Name, "")) / 10;
        }    
    }

    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public static class Notification
    {
        // You do not need to know how these methods work
        public static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = Location.GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
    }

    public class Solution
    {
        static void Main(string[] args)
        {
            /*
             * 1. What should be your approach to getting the list of events?
             * Ideally, this should be a GET call to fetch a list of all events. More specifically, if we already know the customer's citu,
             * this can be another API call which is GET call to fetch a list of events by City.
             * 
             * 2. How would you call the AddToEmail method in order to send the events in an email?
             * AddToEmail should be a service on its own that takes in the customer object and a list of events
             * 
             * 3. What is the expected output if we only have the client John Smith?
               4. Do you believe there is a way to improve the code you first wrote?
             */

            // Customer
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            // answer to number 1
            var eventsByCity = Event.GetEventsByCity(customer.City);

            foreach (var evt in eventsByCity)
            {
                Notification.AddToEmail(customer, evt);
            }

            /**
            * We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
            */
        }

        
        
    }
} /*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/