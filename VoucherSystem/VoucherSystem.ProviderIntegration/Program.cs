using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VoucherSystem.ProviderIntegration.Configuration;
using VoucherSystem.ProviderIntegration.Consumers;
using VoucherSystem.ProviderIntegration.Data;
using VoucherSystem.ProviderIntegration.Interfaces;
using VoucherSystem.ProviderIntegration.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

var mongoSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton<IMongoDatabase>(sp => 
{
    var client = new MongoClient(mongoSettings.ConnectionString);
    return client.GetDatabase(mongoSettings.DatabaseName);
});

builder.Services.AddScoped<IVoucherProvider, DummyVoucherProvider>();


using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    await MongoDbInitializer.SeedData(database);
}


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ListVouchersConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMQSettings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
        
        cfg.Host(rabbitMQSettings.HostName, rabbitMQSettings.Port, "/", h =>
        {
            h.Username(rabbitMQSettings.UserName);
            h.Password(rabbitMQSettings.Password);
        });
    });
});


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