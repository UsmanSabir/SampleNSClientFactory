using Microsoft.Extensions.Caching.Memory;
using Ocelot.Logging;
using Ocelot.Middleware;
using System.Diagnostics;

namespace SampleApiGateway.Custom
{
    public class MyMiddleware : Ocelot.Middleware.OcelotMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;
        private readonly IOcelotLoggerFactory loggerFactory;

        public MyMiddleware(RequestDelegate next, IConfiguration configuration,
            IMemoryCache memoryCache, IOcelotLoggerFactory loggerFactory) 
            : base(loggerFactory.CreateLogger<MyMiddleware>())
        {
            _next = next;
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            this.loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //await _next.Invoke(httpContext);
            //    var permissions = new[]
            //    {
            //   new ApiPermission()
            //   {
            //       PathPattern = "/api/test/values",
            //       Method = "GET",
            //       AllowedRoles = ""
            //   },
            //   new ApiPermission()
            //   {
            //       PathPattern = "/api/test/user",
            //       Method = "GET",
            //       AllowedRoles = "User"
            //   },
            //   new ApiPermission()
            //   {
            //       PathPattern = "/api/test/admin",
            //       Method = "GET",
            //       AllowedRoles = "Admin"
            //   },
            //};

            var route = httpContext.Items.DownstreamRoute();
            var host = route.DownstreamAddresses.FirstOrDefault()?.Host;
            var port = route.DownstreamAddresses.FirstOrDefault()?.Port;
            var method = route.DownstreamPathTemplate.Value;
            var body = httpContext.Request.Body;
            if(route.Key== "sample")
            {
                Debug.WriteLine(route.Key);
            }
            //var routeKeysConfigs = routeKeys.DownstreamRouteConfig;

            var rout= httpContext.GetRouteData();
            //var downstreamRouteHolder = httpContext.Items.DownstreamRouteHolder();
            //if (routeKeysConfigs == null || !routeKeysConfigs.Any())
            {
                //var downstreamRouteHolder = httpContext.Items.DownstreamRouteHolder();

                //var tasks = new Task<HttpContext>[downstreamRouteHolder.Route.DownstreamRoute.Count];

                //for (var i = 0; i < downstreamRouteHolder.Route.DownstreamRoute.Count; i++)
                //{
                //    var newHttpContext = Copy(httpContext);

                //    newHttpContext.Items
                //        .Add("RequestId", httpContext.Items["RequestId"]);
                //    newHttpContext.Items
                //        .SetIInternalConfiguration(httpContext.Items.IInternalConfiguration());
                //    newHttpContext.Items
                //        .UpsertTemplatePlaceholderNameAndValues(httpContext.Items.TemplatePlaceholderNameAndValues());
                //    newHttpContext.Items
                //        .UpsertDownstreamRoute(downstreamRouteHolder.Route.DownstreamRoute[i]);

                //    //tasks[i] = Fire(newHttpContext, _next);
                //}

                //await Task.WhenAll(tasks);
            }
                //var rv= httpContext.GetRouteValue(rout.Values);
                //var downstreamRoute = httpContext.Items.DownstreamRoute();
                //var a = downstreamRoute.AuthenticationOptions.AuthenticationProviderKey;

                //var result = await httpContext.AuthenticateAsync(downstreamRoute.AuthenticationOptions.AuthenticationProviderKey);
                //if (result.Principal != null)
                //{
                //    httpContext.User = result.Principal;
                //}

                //var user = httpContext.User;
                var request = httpContext.Request;

            //httpContext.Items.SetError(new UnauthenticatedError("unauthorized, need login"));

            //var permission = permissions.FirstOrDefault(p =>
            //    request.Path.ToString().Equals(p.PathPattern, StringComparison.OrdinalIgnoreCase) && p.Method.ToUpper() == request.Method.ToUpper());

            //if (permission == null)
            //{
            //    permission =
            //        permissions.FirstOrDefault(p =>
            //            Regex.IsMatch(request.Path.ToString(), p.PathPattern, RegexOptions.IgnoreCase) && p.Method.ToUpper() == request.Method.ToUpper());
            //}

            //if (user.Identity?.IsAuthenticated == true)
            //{
            //    if (!string.IsNullOrWhiteSpace(permission?.AllowedRoles) &&
            //        !permission.AllowedRoles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //            .Any(r => user.IsInRole(r)))
            //    {
            //        httpContext.Items.SetError(new UnauthorizedError("forbidden, have no permission"));
            //        return;
            //    }
            //}
            //else
            //{
            //    if (permission != null && string.IsNullOrWhiteSpace(permission.AllowedRoles))
            //    {
            //    }
            //    else
            //    {
            //        httpContext.Items.SetError(new UnauthenticatedError("unauthorized, need login"));
            //        return;
            //    }
            //}

            await _next.Invoke(httpContext);
        }

        private HttpContext Copy(HttpContext source)
        {
            var target = new DefaultHttpContext();

            foreach (var header in source.Request.Headers)
            {
                target.Request.Headers.TryAdd(header.Key, header.Value);
            }

            target.Request.Body = source.Request.Body;
            target.Request.ContentLength = source.Request.ContentLength;
            target.Request.ContentType = source.Request.ContentType;
            target.Request.Host = source.Request.Host;
            target.Request.Method = source.Request.Method;
            target.Request.Path = source.Request.Path;
            target.Request.PathBase = source.Request.PathBase;
            target.Request.Protocol = source.Request.Protocol;
            target.Request.Query = source.Request.Query;
            target.Request.QueryString = source.Request.QueryString;
            target.Request.Scheme = source.Request.Scheme;
            target.Request.IsHttps = source.Request.IsHttps;
            target.Request.RouteValues = source.Request.RouteValues;
            target.Connection.RemoteIpAddress = source.Connection.RemoteIpAddress;
            target.RequestServices = source.RequestServices;
            return target;
        }

        //private async Task Map(HttpContext httpContext, Route route, List<HttpContext> contexts)
        //{
        //    if (route.DownstreamRoute.Count > 1)
        //    {
        //        var aggregator = _factory.Get(route);
        //        await aggregator.Aggregate(route, httpContext, contexts);
        //    }
        //    else
        //    {
        //        MapNotAggregate(httpContext, contexts);
        //    }
        //}

        //private void MapNotAggregate(HttpContext httpContext, List<HttpContext> downstreamContexts)
        //{
        //    //assume at least one..if this errors then it will be caught by global exception handler
        //    var finished = downstreamContexts.First();

        //    httpContext.Items.UpsertErrors(finished.Items.Errors());

        //    httpContext.Items.UpsertDownstreamRequest(finished.Items.DownstreamRequest());

        //    httpContext.Items.UpsertDownstreamResponse(finished.Items.DownstreamResponse());
        //}

        //private async Task<HttpContext> Fire(HttpContext httpContext, RequestDelegate next)
        //{
        //    await next.Invoke(httpContext);
        //    return httpContext;
        //}
    }
}
