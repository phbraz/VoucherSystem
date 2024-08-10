using MassTransit;
using MongoDB.Driver;
using VoucherSystem.Shared.DTOs;
using VoucherSystem.Shared.Messages;

namespace VoucherSystem.ProviderIntegration.Consumers;

public class ListVouchersConsumer : IConsumer<ListVoucherMessages.ListVouchersRequest>
{
    private readonly IMongoCollection<VoucherDto> _voucherCollection;

    public ListVouchersConsumer(IMongoCollection<VoucherDto> voucherCollection)
    {
        _voucherCollection = voucherCollection;
    }
    
    public async Task Consume(ConsumeContext<ListVoucherMessages.ListVouchersRequest> context)
    {
        Console.WriteLine("--> Consuming ListOfVouchers");

        var vouchers = await _voucherCollection.Find(Builders<VoucherDto>.Filter.Empty).ToListAsync();

        var result = new ListVoucherMessages.ListVouchersResponse(vouchers);

        await context.RespondAsync(result);
    }
}