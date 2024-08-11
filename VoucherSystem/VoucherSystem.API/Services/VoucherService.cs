using MassTransit;
using VoucherSystem.API.Services.Interfaces;
using VoucherSystem.Shared.DTOs;
using VoucherSystem.Shared.Messages;

namespace VoucherSystem.API.Services;

public class VoucherService : IVoucherService
{
    private readonly IRequestClient<ListVoucherMessages.ListVouchersRequest> _listVouchersClient;
    private readonly IRequestClient<SelectVoucherMessage.SelectVoucherRequest> _selectVoucherClient;
    private readonly IRequestClient<CheckoutMessage.CheckoutRequest> _checkoutClient;

    public VoucherService(
        IRequestClient<ListVoucherMessages.ListVouchersRequest> listVouchersClient,
        IRequestClient<SelectVoucherMessage.SelectVoucherRequest> selectVoucherClient,
        IRequestClient<CheckoutMessage.CheckoutRequest> checkoutClient)
    {
        _listVouchersClient = listVouchersClient;
        _selectVoucherClient = selectVoucherClient;
        _checkoutClient = checkoutClient;
    }
    
    public async Task<IEnumerable<VoucherDto>> ListVouchers()
    {
        var response = await _listVouchersClient.GetResponse<ListVoucherMessages.ListVouchersResponse>(new ListVoucherMessages.ListVouchersRequest());
        return response.Message.Vouchers;
    }

    public async Task<VoucherDto> SelectVoucherAndAmount(int voucherId, int amount)
    {
        var response = await _selectVoucherClient.GetResponse<SelectVoucherMessage.SelectVoucherResponse>(new SelectVoucherMessage.SelectVoucherRequest(voucherId, amount));
        return response.Message.SelectedVoucher;
    }

    public async Task Checkout(int cartId)
    {
        await _checkoutClient.GetResponse<CheckoutMessage.CheckoutResponse>(new CheckoutMessage.CheckoutRequest(cartId));
    }
}