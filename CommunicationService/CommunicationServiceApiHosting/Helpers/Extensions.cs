using CommunicationServiceAbstraction;
using CommunicationServiceApiHosting.ServiceClient;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationServiceApiHosting.Helpers;

public static class Extensions
{

    public static IServiceCollection RegisterAsServiceClient<T>(this IServiceCollection services, string serviceId)
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

}