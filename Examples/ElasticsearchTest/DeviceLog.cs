using System;

namespace ElasticsearchTest
{
    public class DeviceLog
    {
        public long CustomerId { get; set; }
        public string VehiclePlate { get; set; }

        public DateTime CreatedTime { get; set; }
        public string Message { get; set; }
    }

}
