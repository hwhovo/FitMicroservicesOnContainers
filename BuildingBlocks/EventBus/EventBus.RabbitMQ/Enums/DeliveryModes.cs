using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.RabbitMQ.Enums
{
    public enum DeliveryModes : byte
    {
        NonPersistent = 1,
        Persistent = 2
    }
}
