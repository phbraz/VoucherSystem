using MassTransit;
using Microsoft.Extensions.Options;
using VoucherSystem.API.Configuration;
using VoucherSystem.API.Messages;
using VoucherSystem.API.Services;
using VoucherSystem.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVoucherService, VoucherService>();

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMQSettings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;
        
        cfg.Host(rabbitMQSettings.HostName, rabbitMQSettings.Port, "/", h =>
        {
            h.Username(rabbitMQSettings.UserName);
            h.Password(rabbitMQSettings.Password);
        });
    });

    // Register request clients
    x.AddRequestClient<VoucherMessages.ListVouchersRequest>();
    x.AddRequestClient<VoucherMessages.SelectVoucherRequest>();
    x.AddRequestClient<VoucherMessages.AddToCartRequest>();
    x.AddRequestClient<VoucherMessages.CheckoutRequest>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();