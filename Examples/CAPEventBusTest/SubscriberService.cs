using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAPEventBusTest
{
    public interface ISubscriberService
    {
        void CheckReceivedMessage(DateTime datetime);
    }

    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        [CapSubscribe("test.show.time")]
        public void CheckReceivedMessage(DateTime datetime)
        {
            Console.WriteLine("message time is:" + datetime);
        }
    }
}
