using MongoDB.Driver;
using VoucherSystem.ProviderIntegration.Interfaces;
using VoucherSystem.Shared.DTOs;
using VoucherSystem.Shared.Messages;

namespace VoucherSystem.ProviderIntegration.Services;

public class DummyVoucherProvider : IVoucherProvider
{
    private readonly IMongoCollection<VoucherDto> _vouchers;

    public DummyVoucherProvider(IMongoDatabase database)
    {
        _vouchers = database.GetCollection<VoucherDto>("Vouchers");
    }
    
    public async Task<ListVoucherMessages.ListVouchersResponse> ListVouchersAsync()
    {
        var items = await _vouchers.Find(_ => true).ToListAsync();
        var result = new ListVoucherMessages.ListVouchersResponse(items);
        return result;
    }

    public async Task<VoucherDto> GetVoucherDetailsAsync(string voucherId)
    {
        return await _vouchers.Find(v => v.Id.ToString() == voucherId).FirstOrDefaultAsync();
    }

    public async Task<SelectVoucherMessage.SelectVoucherResponse> SelectVoucherAndAmountAsync(string voucherId, int amount)
    {
        var voucher = await _vouchers.Find(v => v.Id.ToString() == voucherId && v.Amount >= amount).FirstOrDefaultAsync();
        if (voucher != null)
        {
            var update = Builders<VoucherDto>.Update.Inc(v => v.Amount, -amount);
            await _vouchers.UpdateOneAsync(v => v.Id.ToString() == voucherId, update);
            voucher.Amount = amount;
        }
        
        var result = new SelectVoucherMessage.SelectVoucherResponse(voucher);
        
        return result;
    }

    public async Task<bool> AddToCartAsync(string voucherId, int amount)
    {
        // Implement cart logic here
        return true;
    }

    public async Task<bool> CheckoutAsync(string cartId)
    {
        // Implement checkout logic here
        return true;
    }
}