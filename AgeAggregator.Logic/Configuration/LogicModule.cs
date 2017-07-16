using Autofac;
using System.Reflection;

namespace AgeAggregator.Logic.Configuration
{
    public class LogicModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();            
        }
    }
}
