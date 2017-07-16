using System;

namespace AgeAggregator.App.Services
{
    class ConsoleNotifier : INotifier
    {
        public void NotifyUser(string message)
        {
            Console.WriteLine(message);
        }
    }
}
