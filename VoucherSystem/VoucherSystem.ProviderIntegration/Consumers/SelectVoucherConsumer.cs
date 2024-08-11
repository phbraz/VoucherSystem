using MassTransit;
using VoucherSystem.ProviderIntegration.Interfaces;
using VoucherSystem.Shared.Messages;

namespace VoucherSystem.ProviderIntegration.Consumers;

public class SelectVoucherConsumer : IConsumer<SelectVoucherMessage.SelectVoucherRequest>
{
    private readonly IVoucherProvider _voucherProvider;

    public SelectVoucherConsumer(IVoucherProvider voucherProvider)
    {
        _voucherProvider = voucherProvider;
    }
    
    public async Task Consume(ConsumeContext<SelectVoucherMessage.SelectVoucherRequest> context)
    {
        Console.WriteLine("--> Consuming SelectVoucherAmount");

        var selectedVoucher = await _voucherProvider
            .SelectVoucherAndAmountAsync(context.Message.VoucherId.ToString(), context.Message.Amount);

        await context.RespondAsync(selectedVoucher);
    }
}