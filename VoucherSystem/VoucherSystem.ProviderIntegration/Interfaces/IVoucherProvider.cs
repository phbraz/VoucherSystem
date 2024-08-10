using VoucherSystem.Shared.DTOs;

namespace VoucherSystem.ProviderIntegration.Interfaces;

public interface IVoucherProvider
{
    Task<IEnumerable<VoucherDto>> ListVouchersAsync();
    Task<VoucherDto> GetVoucherDetailsAsync(string voucherId);
    Task<VoucherDto> SelectVoucherAndAmountAsync(string voucherId, int amount);
    Task<bool> AddToCartAsync(string voucherId, int amount);
    Task<bool> CheckoutAsync(string cartId);
}