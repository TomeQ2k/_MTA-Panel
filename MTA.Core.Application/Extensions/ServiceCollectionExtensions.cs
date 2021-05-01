using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MTA.Core.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly[] assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies =
                assemblies.SelectMany(a =>
                    a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T)) && !x.IsAbstract));

            foreach (var assemblyType in typesFromAssemblies)
                services.Add(new ServiceDescriptor(typeof(T), assemblyType, lifetime));
        }
    }
}