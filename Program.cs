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

    public static class EmailTemplate
    {
        public static string GeneralTemplate(Customer c, Event e, int distance, int? price = null) => $"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : "");
    }

    public static class Notification
    {
        // You do not need to know how these methods work
        public static void AddToEmail(Customer c, List<Event> es, int? price = null)
        {
            foreach(var e in es)
            {
                var emailTemplate = EmailTemplate.GeneralTemplate(c, e, Location.GetDistance(c.City, e.City));
                Console.Out.WriteLine(emailTemplate);
            }
        }
    }

    public static class Stubhub
    {
        public static void SendEmailByCity(Customer c, List<Event> eventsByCity)
        {
            if (eventsByCity.Count > 0)
                Notification.AddToEmail(c, eventsByCity);
            else
            {
                // log failure
            }
        }

        public static Dictionary<Event, int> GetEventDistancePerCity(Customer c, int limit = 0)
        {
            var dictDistance = new Dictionary<Event, int>();
            try
            {
                var events = Event.GetEvents();
                foreach (Event e in events)
                {
                    dictDistance.Add(e, Location.GetDistance(c.City, e.City));
                }

                dictDistance = dictDistance.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                if (limit > 0)
                    dictDistance = dictDistance.Take(5).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                // log failure
            }

            return dictDistance;
        }

        public static Dictionary<Event, int> GetPricePerEvent(bool asc = false)
        {
            var dictEventsAndPrices = new Dictionary<Event, int>();
            try
            {
                var events = Event.GetEvents();
                foreach (Event e in events)
                {
                    dictEventsAndPrices.Add(e, Event.GetPrice(e));
                }

                if (asc)
                    dictEventsAndPrices = dictEventsAndPrices.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                else
                    dictEventsAndPrices = dictEventsAndPrices.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                // log failure
            }

            return dictEventsAndPrices;
        }
    }

    public class Solution
    {
        static void Main(string[] args)
        {

            // Customer
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            // 1
            var eventsByCity = Event.GetEventsByCity(customer.City);
            Stubhub.SendEmailByCity(customer, eventsByCity);

            // 2
            Stubhub.GetEventDistancePerCity(customer);

            // 3
            var eventsByDistance = Stubhub.GetEventDistancePerCity(customer, 5).Select(x => x.Key).ToList();
            Stubhub.SendEmailByCity(customer, eventsByDistance);

            // 4
            var eventsByPrice = Stubhub.GetPricePerEvent().Select(x => x.Key).ToList();
            Stubhub.SendEmailByCity(customer, eventsByPrice);

            // I can unit test each class to ensure the expected output is obtained
        }
    }
}