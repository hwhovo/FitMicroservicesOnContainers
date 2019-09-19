using ChatRoom.Core.Abstractions.OperationInterfaces;
using ChatRoom.Core.Entites;
using ChatRoom.Core.IntegrationEvents.Events;
using EventBus.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.BLL.IntegrationEventHandlers
{
    public class AddUserIntegrationEventHandler
        : IIntegrationEventHandler<AddUserIntegrationEvent>
    {
        private IUserOperations _userOperations;

        public AddUserIntegrationEventHandler(IUserOperations userOperations)
        {
            _userOperations = userOperations;
        }

        public async Task Handle(AddUserIntegrationEvent @event)
        {
            var user = new User
            {
                Username = @event.Username,
                LastName = @event.LastName,
                FirstName = @event.FirstName
            };

            await _userOperations.CreateAsync(user, @event.Password);
        }
    }
}
