using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Models.HealthCheck
{
    public class HealthCheckResponseModel
    {
        public string Status { get; set; }

        public IEnumerable<HealthCheckModel> Checks { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
