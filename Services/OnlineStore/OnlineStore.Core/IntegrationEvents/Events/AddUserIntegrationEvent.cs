using EventBus.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.IntegrationEvents.Events
{
    public class AddUserIntegrationEvent : IntegrationEvent
    {
        public AddUserIntegrationEvent(string id, string firstName, string lastName, string username, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
