using System.Threading.Tasks;

namespace EventBus.Core.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle<T>(T eventData);
    }
}
