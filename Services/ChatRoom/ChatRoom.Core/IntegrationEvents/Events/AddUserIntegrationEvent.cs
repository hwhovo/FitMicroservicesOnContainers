using EventBus.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Core.IntegrationEvents.Events
{
    public class AddUserIntegrationEvent : IntegrationEvent
    {
        public AddUserIntegrationEvent()
        {

        }
        public AddUserIntegrationEvent(string firstName, string lastName, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
