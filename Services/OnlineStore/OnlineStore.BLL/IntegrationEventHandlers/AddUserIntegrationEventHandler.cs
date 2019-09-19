using EventBus.Core.Abstractions;
using OnlineStore.BLL.Operations;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using OnlineStore.Core.Entites;
using OnlineStore.Core.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.IntegrationEventHandlers
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
                UserName = @event.Username,
                LastName = @event.LastName,
                FirstName = @event.FirstName
            };

            await _userOperations.CreateAsync(user, @event.Password);
        }
    }
}
