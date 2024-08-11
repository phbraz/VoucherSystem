using MongoDB.Driver;
using VoucherSystem.ProviderIntegration.Interfaces;
using VoucherSystem.ProviderIntegration.Models;
using VoucherSystem.Shared.DTOs;
using VoucherSystem.Shared.Enums;
using VoucherSystem.Shared.Messages;

namespace VoucherSystem.ProviderIntegration.Services;

public class DummyVoucherProvider : IVoucherProvider
{
    private readonly IMongoCollection<VoucherDto> _vouchers;
    private readonly IMongoCollection<Cart> _carts;

    public DummyVoucherProvider(IMongoDatabase database)
    {
        _vouchers = database.GetCollection<VoucherDto>("Vouchers");
        _carts = database.GetCollection<Cart>("Carts");
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
            await AddToCartAsync(voucher);
        }
        
        var result = new SelectVoucherMessage.SelectVoucherResponse(voucher);
        
        return result;
    }

    public async Task<bool> AddToCartAsync(VoucherDto voucher)
    {

        var cart = await _carts.Find(c => c.UserId == 1 && c.Status == CartStatus.InProgress).FirstOrDefaultAsync();
        if (cart == null)
        {
            cart = new Cart()
            {
                UserId = 1, //hard coding the user assuming we do have an identity service in place
                Status = CartStatus.InProgress,
                Items = new List<VoucherDto>() { voucher },
                Price = voucher.Value * voucher.Amount
            };
            await _carts.InsertOneAsync(cart);
        }
        else
        {
            var update = Builders<Cart>.Update
                .Push(c => c.Items, voucher)
                .Inc(c => c.Price, voucher.Value * voucher.Amount);
            
            await _carts.UpdateOneAsync(c => c.UserId == 1, update);
        }
        
        return true;
    }

    //again I'm not adding identity to this project hence we are hard coding the userid
    public async Task<CheckoutMessage.CheckoutResponse> CheckoutAsync()
    {
        var cartItems = await _carts.Find(c => c.UserId == 1 && c.Status == CartStatus.InProgress)
            .FirstOrDefaultAsync();

        if (cartItems != null)
        {
            var update = Builders<Cart>.Update.Set(c => c.Status, CartStatus.Completed);
            var updateResult = await _carts.UpdateOneAsync(
                c => c.UserId == 1 && c.Status == CartStatus.InProgress,
                update);

            return new CheckoutMessage.CheckoutResponse(true);
            //here we can add additional tasks? i.e: Send confirmation email, Generate Order ID, And so on. 
        }
        else
        {
            //we do not have a cart items then we can't checkout.
            return new CheckoutMessage.CheckoutResponse(false);
        }
    }
}