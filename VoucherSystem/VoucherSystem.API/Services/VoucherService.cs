using MassTransit;
using VoucherSystem.API.DTOs;
using VoucherSystem.API.Messages;
using VoucherSystem.API.Services.Interfaces;

namespace VoucherSystem.API.Services;

public class VoucherService : IVoucherService
{
    private readonly IRequestClient<VoucherMessages.ListVouchersRequest> _listVouchersClient;
    private readonly IRequestClient<VoucherMessages.SelectVoucherRequest> _selectVoucherClient;
    private readonly IRequestClient<VoucherMessages.AddToCartRequest> _addToCartClient;
    private readonly IRequestClient<VoucherMessages.CheckoutRequest> _checkoutClient;

    public VoucherService(
        IRequestClient<VoucherMessages.ListVouchersRequest> listVouchersClient,
        IRequestClient<VoucherMessages.SelectVoucherRequest> selectVoucherClient,
        IRequestClient<VoucherMessages.AddToCartRequest> addToCartClient,
        IRequestClient<VoucherMessages.CheckoutRequest> checkoutClient)
    {
        _listVouchersClient = listVouchersClient;
        _selectVoucherClient = selectVoucherClient;
        _addToCartClient = addToCartClient;
        _checkoutClient = checkoutClient;
    }
    
    public async Task<IEnumerable<VoucherDto>> ListVouchers()
    {
        var response = await _listVouchersClient.GetResponse<VoucherMessages.ListVouchersResponse>(new VoucherMessages.ListVouchersRequest());
        return response.Message.Vouchers;
    }

    public async Task<VoucherDto> SelectVoucherAndAmount(int voucherId, int amount)
    {
        var response = await _selectVoucherClient.GetResponse<VoucherMessages.SelectVoucherResponse>(new VoucherMessages.SelectVoucherRequest(voucherId, amount));
        return response.Message.SelectedVoucher;
    }

    public async Task AddToCart(int voucherId, int amount)
    {
        await _addToCartClient.GetResponse<VoucherMessages.AddToCartResponse>(new VoucherMessages.AddToCartRequest(voucherId, amount));
    }

    public async Task Checkout(int cartId)
    {
        await _checkoutClient.GetResponse<VoucherMessages.CheckoutResponse>(new VoucherMessages.CheckoutRequest(cartId));
    }
}