using MassTransit;
using VoucherSystem.ProviderIntegration.Interfaces;
using VoucherSystem.Shared.Messages;

namespace VoucherSystem.ProviderIntegration.Consumers;

public class CheckoutConsumer : IConsumer<CheckoutMessage.CheckoutRequest>
{
    private readonly IVoucherProvider _voucherProvider;

    public CheckoutConsumer(IVoucherProvider voucherProvider)
    {
        _voucherProvider = voucherProvider;
    }
    
    public async Task Consume(ConsumeContext<CheckoutMessage.CheckoutRequest> context)
    {
        Console.WriteLine("--> Consuming Checkout");

        var isCheckoutCompleted = await _voucherProvider.CheckoutAsync();

        await context.RespondAsync(isCheckoutCompleted);
    }
}