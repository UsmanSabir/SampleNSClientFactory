using System.Reflection;
using CommunicationServiceAbstraction;
using CommunicationServiceApiFramework.ServiceClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CommunicationServiceApiFramework;

public static class Extensions
{
    #region Framework

    public static IServiceCollection AddCommunicationFramework(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IClientFactory, ClientFactoryImpl>();
        services.AddScoped<IServiceAddressResolver, ConfigFileServiceAddressResolver>();

        services.AddHttpClient();
        var clientAddresses = config.GetSection("services").AsEnumerable();
        foreach (var address in clientAddresses)
        {
            //todo param checks
            services.AddHttpClient(address.Key, client =>
            {
                client.BaseAddress = new Uri(address.Value);
                
            });
            //todo polly
        }
        return services;
    }

    #endregion

    #region Client Services


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

    public static IServiceCollection RegisterClientBusinessServices(this IServiceCollection services, ServiceIdentities serviceId, params Assembly[] assemblies)
    {
        var type = typeof(Extensions);
        var methodInfo = type.GetMethod(nameof(RegisterAsServiceClient), BindingFlags.Public | BindingFlags.Static);


        var bizType = typeof(IBusinessService);

        foreach (TypeInfo ti in assemblies.SelectMany(s => s.DefinedTypes))
        {
            var typeArguments = ti.AsType();
            if (ti.IsInterface && ti.IsPublic && typeArguments != bizType
                && ti.ImplementedInterfaces.Contains(bizType))
            {
                var genericMethod = methodInfo.MakeGenericMethod(typeArguments);
                var invoke = genericMethod.Invoke(null, new object[] { services, serviceId });
            }
        }

        return services;
    }


    #endregion


}