using System.Threading.Tasks;

namespace EventBus.Core.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
