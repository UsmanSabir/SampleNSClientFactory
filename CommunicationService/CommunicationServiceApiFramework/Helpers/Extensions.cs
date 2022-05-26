using System.Reflection;
using CommunicationServiceAbstraction;
using CommunicationServiceApiFramework.ServiceClient;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationServiceApiFramework.Helpers;

public static class Extensions
{
    public static IServiceCollection RegisterAsServiceClient<T>(this IServiceCollection services, ServiceIdentities serviceId)
        where T : IBusinessService
    {
        services.AddScoped(typeof(T), (di) =>
        {
            var businessService = ProxyDecorator<T>.Decorate(di, serviceId);
            return businessService;
        });

        //this doesn't work
        //services.AddScoped<T>((di) =>
        //{
        //    var businessService = ProxyDecorator<T>.Decorate(di);
        //    return businessService;
        //});

        return services;
    }

    static IServiceCollection RegisterClientBusinessServices(this IServiceCollection services, ServiceIdentities serviceId, params Assembly[] assemblies)
    {
        var type = typeof(Extensions);
        var methodInfo = type.GetMethod(nameof(RegisterAsServiceClient), BindingFlags.Public | BindingFlags.Static);


        var bizType = typeof(IBusinessService);

        foreach (TypeInfo ti in assemblies.SelectMany(s => s.DefinedTypes))
        {
            if (ti.IsClass && !ti.IsAbstract
                && ti.ImplementedInterfaces.Contains(bizType))
            {
                var genericMethod = methodInfo.MakeGenericMethod(new[] { ti.GetType() });
                var invoke = genericMethod.Invoke(null, new object[] { services, serviceId });
            }
        }

        return services;
    }

}