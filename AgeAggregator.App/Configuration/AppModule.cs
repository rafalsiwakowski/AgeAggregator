using AgeAggregator.App.Services;
using Autofac;

namespace AgeAggregator.App.Configuration
{
    class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AgeAggregatorService>().AsSelf();
            builder.RegisterType<ConsoleNotifier>().As<INotifier>();
        }
    }
}
