using MassTransit;
using MongoDB.Driver;
using VoucherSystem.ProviderIntegration.Interfaces;
using VoucherSystem.Shared.DTOs;
using VoucherSystem.Shared.Messages;

namespace VoucherSystem.ProviderIntegration.Consumers;

public class ListVouchersConsumer : IConsumer<ListVoucherMessages.ListVouchersRequest>
{
    private readonly IVoucherProvider _voucherProvider;

    public ListVouchersConsumer(IVoucherProvider voucherProvider)
    {
        _voucherProvider = voucherProvider;
    }
    
    public async Task Consume(ConsumeContext<ListVoucherMessages.ListVouchersRequest> context)
    {
        Console.WriteLine("--> Consuming ListOfVouchers");

        var vouchers = await _voucherProvider.ListVouchersAsync();

        await context.RespondAsync(vouchers);
    }
}