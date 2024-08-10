using MassTransit;
using Microsoft.Extensions.Options;
using VoucherSystem.API.Configuration;
using VoucherSystem.API.Services;
using VoucherSystem.API.Services.Interfaces;
using VoucherSystem.Shared.Messages;

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
    x.AddRequestClient<ListVoucherMessages.ListVouchersRequest>();
    x.AddRequestClient<SelectVoucherMessage.SelectVoucherRequest>();
    x.AddRequestClient<AddToCartMessage.AddToCartRequest>();
    x.AddRequestClient<CheckoutMessage.CheckoutRequest>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();