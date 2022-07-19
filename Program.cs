using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Viagogo
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class EventWithDistance : Event
    {
        public int Distance { get; set; }
    }

    public class EventWithPrice : Event
    {
        public int Price { get; set; }
    }

    public class EventWithPriceAndDistance : Event
    {
        public int Distance { get; set; }
        public int Price { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Solution
    {
        static void Main(string[] args)
        {
            var events = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
             };

            //1. find out all events that arein cities of customer
            // then add to email.
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };
            SendEventsInCustomerCity(events, customer);

            /**
            * We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
            */

            // What should be your approach to getting the distance between the customer’s city and the other cities on the list?
            SendClosestEventsWithLimit(events, customer, 5);

            // If the GetDistance method is an API call which could fail or is too expensive, how will u improve the code written in 2 ?

            // If we also want to sort the resulting events by other fields like price
            SendEventByPriceFilter(events, customer, 0, 10);
        }

        public static List<EventWithPrice> SendEventByPriceFilter(List<Event> events, Customer customer, int min = 0, int max = 0)
        {
            var eventsAndDistance = events.Select(x => new EventWithPrice()
            {
                Name = x.Name,
                City = customer.City,
                Price = GetPrice(x)
            }).ToList();

            // How would you get the 5 closest events and how would you send them to the client in an email?
            var closestEvents = eventsAndDistance.Where(e => e.Price >= min && e.Price <= max).ToList();
           
            foreach (var item in closestEvents)
            {
                AddToEmail(customer, item);
            }

            return eventsAndDistance;
        }

        public static List<EventWithDistance> SendClosestEventsWithLimit(List<Event> events, Customer customer, int limit)
        {
            var eventsAndDistance = events.Select(x => new EventWithDistance()
            {
                Name = x.Name,
                City = customer.City,
                Distance = GetDistance(customer.City, x.City)
            }).ToList();

            // How would you get the 5 closest events and how would you send them to the client in an email?
            var closestEvents = new List<EventWithDistance>();
            if (limit > 0)
                closestEvents = eventsAndDistance.OrderBy(e => e.Distance).Take(limit).ToList();
            else
                closestEvents = eventsAndDistance.OrderBy(e => e.Distance).ToList();
            
            foreach (var item in closestEvents)
            {
                AddToEmail(customer, item);
            }

            return eventsAndDistance;
        }

        public static List<Event> SendEventsInCustomerCity(List<Event> events, Customer customer)
        {
            var evensInCustomersCity = from evt in events
                                       where evt.City.Equals("New York")
                                       select evt;
            // 1. TASK
            foreach (var item in evensInCustomersCity)
            {
                AddToEmail(customer, item);
            }

            return evensInCustomersCity;
        }

        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        static int GetDistance(string fromCity, string toCity)
        {
            try
            {
                return AlphebiticalDistance(fromCity, toCity);
            }
            catch (Exception ex)
            {
                // log error
                return 0;
            }
        }
        private static int AlphebiticalDistance(string s, string t)
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
}