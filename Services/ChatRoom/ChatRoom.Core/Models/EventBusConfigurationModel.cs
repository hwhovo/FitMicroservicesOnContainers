using System;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.Core.Models
{
    public class EventBusConfigurationModel
    {
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? RetryCount { get; set; }
        public string SubscriptionQueueName { get; set; }
        public string ConnectionName { get; set; }
    }
}
