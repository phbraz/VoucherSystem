using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VoucherSystem.ProviderIntegration.Configuration;
using VoucherSystem.ProviderIntegration.Consumers;
using VoucherSystem.ProviderIntegration.Data;
using VoucherSystem.ProviderIntegration.Interfaces;
using VoucherSystem.ProviderIntegration.Services;
using VoucherSystem.Shared.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var mongoSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton<IMongoDatabase>(sp => 
{
    var client = new MongoClient(mongoSettings.ConnectionString);
    return client.GetDatabase(mongoSettings.DatabaseName);
});
builder.Services.AddSingleton<IMongoCollection<VoucherDto>>(sp =>
{
    var client = new MongoClient(mongoSettings.ConnectionString);
    var database = client.GetDatabase(mongoSettings.DatabaseName);
    return database.GetCollection<VoucherDto>("Vouchers");
});

builder.Services.AddScoped<IVoucherProvider, DummyVoucherProvider>();
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ListVouchersConsumer>();
    x.AddConsumer<SelectVoucherConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMQSettings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
        
        cfg.Host(rabbitMQSettings.HostName, rabbitMQSettings.Port, "/", h =>
        {
            h.Username(rabbitMQSettings.UserName);
            h.Password(rabbitMQSettings.Password);
        });
        
        cfg.ReceiveEndpoint("vouchers", e =>
        {
            e.UseMessageRetry(r => r.Interval(5,5));
            e.ConfigureConsumer<ListVouchersConsumer>(context);
        });
        
        cfg.ReceiveEndpoint("select", e =>
        {
            e.UseMessageRetry(r => r.Interval(5,5));
            e.ConfigureConsumer<SelectVoucherConsumer>(context);
        });
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    await MongoDbInitializer.SeedData(database);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();