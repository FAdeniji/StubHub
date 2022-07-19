using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Viagogo;

namespace StubHub.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly List<Event> events = new List<Event>{
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

        private readonly Customer customer = new Customer { Name = "Mr. Fake", City = "New York" };

        [TestMethod]
        public void Check_the_right_number_of_events_in_customer_city_was_sent()
        {
            var result = Viagogo.Solution.SendEventsInCustomerCity(events, customer);
            Assert.AreEqual(result.Count, 3);
        }
    }
}
