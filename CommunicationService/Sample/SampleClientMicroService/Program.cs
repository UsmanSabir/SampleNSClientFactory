using CommunicationServiceAbstraction;
using CommunicationServiceApiFramework;
using Sample.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCommunicationFramework();

builder.Services.RegisterAsServiceClient<IBusinessPartnerService>(ServiceIdentities.UMS);
builder.Services.RegisterClientBusinessServices(ServiceIdentities.UMS,
    typeof(IBusinessPartnerService).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
