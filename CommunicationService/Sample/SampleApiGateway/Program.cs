using Ocelot.Claims.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.DownstreamPathManipulation.Middleware;
using Ocelot.DownstreamRouteFinder.Middleware;
using Ocelot.DownstreamUrlCreator.Middleware;
using Ocelot.Errors.Middleware;
using Ocelot.Headers.Middleware;
using Ocelot.LoadBalancer.Middleware;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using Ocelot.QueryStrings.Middleware;
using Ocelot.RateLimit.Middleware;
using Ocelot.Request.Middleware;
using Ocelot.Requester.Middleware;
using Ocelot.RequestId.Middleware;
using Ocelot.Responder.Middleware;
using Ocelot.Security.Middleware;
using SampleApiGateway.Custom;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddOcelot();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var configuration = new OcelotPipelineConfiguration
{
    
    PreErrorResponderMiddleware = async (ctx, next) =>
    {
        await next.Invoke();
    }
};

app.UseOcelot(
    (ocelotBuilder, pipelineConfiguration) =>
{
    // this sets up the downstream context and gets the config
    app.UseDownstreamContextMiddleware();
    // This is registered to catch any global exceptions that are not handled
    // It also sets the Request Id if anything is set globally
    ocelotBuilder.UseExceptionHandlerMiddleware();

    // This is registered first so it can catch any errors and issue an appropriate response
    ocelotBuilder.UseResponderMiddleware();
    //ocelotBuilder.UseMiddleware<CustomResponseMiddleware>();

    ocelotBuilder.UseDownstreamRouteFinderMiddleware();
    ocelotBuilder.UseMultiplexingMiddleware();

    // This security module, IP whitelist blacklist, extended security mechanism
    ocelotBuilder.UseSecurityMiddleware();

    ocelotBuilder.UseHttpHeadersTransformationMiddleware();

    ocelotBuilder.UseDownstreamRequestInitialiser();

    // We check whether the request is ratelimit, and if there is no continue processing
    ocelotBuilder.UseRateLimiting();

    ocelotBuilder.UseRequestIdMiddleware();

    // ocelotBuilder.Use((ctx, next) =>
    // {
    //     ctx.Items.SetError(new UnauthorizedError("No permission"));
    //     return Task.CompletedTask;
    // });

    //ocelotBuilder.UseMiddleware<UrlBasedAuthenticationMiddleware>();

    // The next thing we do is look at any claims transforms in case this is important for authorization
    ocelotBuilder.UseClaimsToClaimsMiddleware();

    // Now we can run the claims to headers transformation middleware
    ocelotBuilder.UseClaimsToHeadersMiddleware();

    // Allow the user to implement their own query string manipulation logic
    ocelotBuilder.UseIfNotNull(pipelineConfiguration.PreQueryStringBuilderMiddleware);

    // Now we can run any claims to query string transformation middleware
    ocelotBuilder.UseClaimsToQueryStringMiddleware();

    ocelotBuilder.UseClaimsToDownstreamPathMiddleware();

    ocelotBuilder.UseLoadBalancingMiddleware();
    ocelotBuilder.UseDownstreamUrlCreatorMiddleware();

    ocelotBuilder.UseMiddleware<MyMiddleware>();

    ocelotBuilder.UseHttpRequesterMiddleware();


    //=-=================================

    // this sets up the downstream context and gets the config
    //app.UseDownstreamContextMiddleware();
    //// This is registered to catch any global exceptions that are not handled
    //// It also sets the Request Id if anything is set globally
    //ocelotBuilder.UseExceptionHandlerMiddleware();

    //// This is registered first so it can catch any errors and issue an appropriate response
    ////ocelotBuilder.UseResponderMiddleware();
    ////ocelotBuilder.UseMiddleware();

    //ocelotBuilder.UseDownstreamRouteFinderMiddleware();
    //ocelotBuilder.UseMultiplexingMiddleware();
    //ocelotBuilder.UseDownstreamRequestInitialiser();
    //ocelotBuilder.UseRequestIdMiddleware();


    //ocelotBuilder.UseMiddleware<MyMiddleware>();

    //ocelotBuilder.UseLoadBalancingMiddleware();
    //ocelotBuilder.UseDownstreamUrlCreatorMiddleware();
    //ocelotBuilder.UseHttpRequesterMiddleware();
}

).Wait();

app.MapControllers();

app.Run();

public static class Extensions
{
    public static void UseIfNotNull(this IApplicationBuilder builder,
        Func<HttpContext, Func<Task>, Task> middleware)
    {
        if (middleware != null)
        {
            builder.Use(middleware);
        }
    }
}