using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using CommunicationServiceAbstraction;
using CommunicationServiceApiFramework.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommunicationServiceApiFramework.ServiceClient;

internal class ProxyDecorator<T> : DispatchProxy where T : IBusinessService
{
    private ServiceIdentities _serviceId;
    IHttpClientFactory _httpClientFactory = null!;
    private IServiceAddressResolver? _serviceAddressResolver = null;
    private ILogger<ProxyDecorator<T>>? _logger;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        try
        {
            if (targetMethod == null)
            {
                //todo : log
                return null;
            }
            //todo
            //var result = targetMethod.Invoke(Target, args);
            //return result;

            //route to _serviceId url
            var httpClient = _httpClientFactory.CreateClient(_serviceId.ToString());
            if (string.IsNullOrWhiteSpace(httpClient?.BaseAddress?.AbsoluteUri))
            {
                //something wrong
                throw new InvalidOperationException($"Base uri not set for service identity '{_serviceId}'");
            }

            var serviceType = typeof(T).Name;
            var serviceEndpoint = _serviceAddressResolver?.GetBusinessServiceEndpoint(serviceType);
            if (string.IsNullOrWhiteSpace(serviceEndpoint))
            {
                //something wrong
                throw new InvalidOperationException($"Couldn't resolve service endpoint'{serviceType}'");
            }

            try
            {
                string? argsJson = null;
                if (args is { Length: > 0 })
                {
                    argsJson = JsonSerializer.Serialize(args);
                }

                var requestModel = new RequestModel()
                {
                    TypeName = targetMethod.DeclaringType?.Name,
                    MethodName = targetMethod.Name,
                    Args = argsJson,
                };
                var response = httpClient.PostAsJsonAsync(serviceEndpoint, requestModel)
                    .Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseModel = response.Content.ReadFromJsonAsync<ResponseModel>().Result;
                    if (responseModel != null)
                    {
                        _logger?.LogDebug($"Response received. Success '{responseModel.IsSuccess}', Error: '{responseModel.Error}' and response: '{responseModel.Response}'");
                        //response model
                        if (responseModel.IsSuccess)
                        {
                            if (targetMethod.ReturnType != typeof(void))
                            {
                                var responseObj =
                                    JsonSerializer.Deserialize(responseModel.Response, targetMethod.ReturnType);
                                return responseObj;
                            }
                        }
                        else
                        {
                            throw new RemoteException(responseModel.Error);
                        }
                    }
                }
            }
            catch (AggregateException ae)
            {
                //todo
                _logger?.LogError(ae.ToString());
                throw ae.InnerException!;
            }
        }
        catch (TargetInvocationException exc)
        {
            _logger?.LogError(exc.InnerException?.ToString());
            throw exc.InnerException!;
        }
    }


    public static T Decorate(IServiceProvider serviceProvider, ServiceIdentities serviceId)
    {
        var proxy = Create<T, ProxyDecorator<T>>();
        
        var proxyDecorator = (proxy as ProxyDecorator<T>);
        if (proxyDecorator != null)
        {
            proxyDecorator._serviceId = serviceId;
            proxyDecorator._logger = serviceProvider.GetService<ILogger<ProxyDecorator<T>>>();
            proxyDecorator._httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            proxyDecorator._serviceAddressResolver = serviceProvider.GetRequiredService<IServiceAddressResolver>();
        }

        return proxy;
    }
}