using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Threading.Timer;

namespace MultiThreadingTest
{
    //https://docs.microsoft.com/en-us/dotnet/api/system.threading.timer
    public class TimerTest
    {
        //Multithreaded Timers
        //https://docs.microsoft.com/en-us/dotnet/api/system.threading.timer
        //https://docs.microsoft.com/en-us/dotnet/api/system.timers.timer

        //Single-Threaded Timers
        //System.Windows.Threading.DispatcherTimer (WPF)
        //System.Windows.Forms.Timer (Windows Forms)

        public static void RunSystemThreadingTimer()
        {
            var timer = new SystemThreadingTimer(TimerTick);
            timer.Interval = 2000;
            timer.Start();
            Thread.Sleep(10000);
            timer.Stop();
        }

        private static void TimerTick(object? state)
        {
            Console.WriteLine(DateTime.Now);
        }

        public static void RunSystemTimersTimer()
        {
            var timer = new SystemTimersTimer((o, args) => TimerTick(null));
            timer.Interval = 2000;
            timer.Start();
            Thread.Sleep(10000);
            timer.Stop();
        }

    }

    public class SystemThreadingTimer
    {
        private readonly Timer _timer;

        public int Interval { get; set; } = 1000;

        public SystemThreadingTimer(TimerCallback callback)
        {
            _timer = new Timer(callback, null, Timeout.Infinite, Interval);
        }

        public void Start()
        {
            _timer.Change(0, Interval);
            Console.WriteLine("Timer started");
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            Console.WriteLine("Timer stopped");
        }
    }

    public class SystemTimersTimer
    {
        private readonly System.Timers.Timer _timer;

        public int Interval { get; set; } = 1000;

        public SystemTimersTimer(Action<object, ElapsedEventArgs> callback)
        {
            _timer = new System.Timers.Timer()
            {
                Interval = this.Interval,
                AutoReset = true
            };

            _timer.Elapsed += callback.Invoke;
        }

        public void Start()
        {
            _timer.Interval = Interval;
            _timer.Enabled = true;
            //_timer.Start();
            Console.WriteLine("Timer started");
        }

        public void Stop()
        {
            _timer.Enabled = false;
            //_timer.Stop();
            Console.WriteLine("Timer stopped");
        }
    }
}
