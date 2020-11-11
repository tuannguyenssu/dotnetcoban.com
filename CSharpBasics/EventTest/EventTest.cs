using System;
using System.Collections.Generic;

namespace EventTest
{
    public class SampleEventArgs
    {
        public SampleEventArgs(string s) { Text = s; }
        public string Text { get; }
    }
    public class Subscriber
    {
        public void Subscribe(Publisher publisher)
        {
            publisher.RegisterSubscriber(this);
            publisher.SampleEvent += PublisherOnSampleEvent;
        }

        private void PublisherOnSampleEvent(object sender, SampleEventArgs e)
        {
            Console.WriteLine(e.Text);
        }
    }

    public class Publisher
    {
        public delegate void SampleEventHandler(object sender, SampleEventArgs e);

        public event SampleEventHandler SampleEvent;

        private readonly List<Subscriber> _subscribers = new List<Subscriber>();
        public void RegisterSubscriber(Subscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void Publish()
        {
            var message = "Sample data";
            _subscribers.ForEach(s =>
            {
                SampleEvent?.Invoke(this, new SampleEventArgs(message));
            });
        }

    }
    public class EventTest
    {
        public static void Run()
        {
            var subscriber = new Subscriber();
            var publisher = new Publisher();
            subscriber.Subscribe(publisher);

            publisher.Publish();
        }

        public static void RunWithDotnetEvent()
        {
            var serialPort = new SerialPort();
            serialPort.DataReceived += DataReceived;
            serialPort.TestEvent();
        }

        private static void DataReceived(object sender, SerialDataEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }

    public class SerialDataEventArgs : EventArgs
    {
        public string Data { get; }

        public SerialDataEventArgs(string data)
        {
            Data = data;
        }
    }

    public class SerialPort
    {
        public EventHandler<SerialDataEventArgs> DataReceived;

        public void TestEvent()
        {
            DataReceived?.Invoke(this, new SerialDataEventArgs(Guid.NewGuid().ToString()));
        }
    }
}
