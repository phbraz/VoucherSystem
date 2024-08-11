using VoucherSystem.Shared.DTOs;
using VoucherSystem.Shared.Messages;

namespace VoucherSystem.ProviderIntegration.Interfaces;

public interface IVoucherProvider
{
    Task<ListVoucherMessages.ListVouchersResponse> ListVouchersAsync();
    Task<VoucherDto> GetVoucherDetailsAsync(string voucherId);
    Task<SelectVoucherMessage.SelectVoucherResponse> SelectVoucherAndAmountAsync(string voucherId, int amount);
    Task<bool> AddToCartAsync(VoucherDto voucher);
    Task<bool> CheckoutAsync();
}