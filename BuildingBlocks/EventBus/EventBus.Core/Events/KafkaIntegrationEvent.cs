namespace EventBus.Core.Events
{
    public class KafkaIntegrationEvent<TKey, TValue> : IntegrationEvent
    {
        public string Topic { get; set; }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }
}
