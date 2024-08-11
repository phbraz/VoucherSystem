using VoucherSystem.Shared.DTOs;

namespace VoucherSystem.API.Services.Interfaces;

public interface IVoucherService
{
    Task<IEnumerable<VoucherDto>> ListVouchers();
    Task<VoucherDto> SelectVoucherAndAmount(int voucherId, int amount);
    Task<bool> Checkout();
}