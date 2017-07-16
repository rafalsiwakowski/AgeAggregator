using AgeAggregator.App.Configuration;
using AgeAggregator.App.Services;
using AgeAggregator.Logic.Configuration;
using Autofac;
using System;

namespace AgeAggregator.App
{
    class Program
    {
        public static void Main(string[] args)
        {
            var container = BuildContainer();
            container.Resolve<AgeAggregatorService>().RunAsync(args[0], args[1]);
            Console.Read();
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AppModule>();
            builder.RegisterModule<LogicModule>();
            return builder.Build();
        }
    }
}
