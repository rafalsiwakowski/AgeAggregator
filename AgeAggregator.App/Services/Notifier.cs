using System;

namespace AgeAggregator.App.Services
{
    public interface INotifier
    {
        void NotifyUser(string message);
    }

    class ConsoleNotifier : INotifier
    {
        public void NotifyUser(string message)
        {
            Console.WriteLine(message);
        }
    }
}
