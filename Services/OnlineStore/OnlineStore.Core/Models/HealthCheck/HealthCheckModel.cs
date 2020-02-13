using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Models.HealthCheck
{
    public class HealthCheckModel
    {
        public string Status { get; set; }

        public string Component { get; set; }

        public string Description { get; set; }
    }
}
