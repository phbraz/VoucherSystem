using MassTransit;
using VoucherSystem.API.Messages;

namespace VoucherSystem.ProviderIntegration.Consumers;

public class ListVouchersConsumer : IConsumer<VoucherMessages.ListVouchersResponse>
{
    public async Task Consume(ConsumeContext<VoucherMessages.ListVouchersResponse> context)
    {
        Console.WriteLine("--> Consuming ListOfVouchers");

        await context.Publish(context.Message.Vouchers);
    }
}