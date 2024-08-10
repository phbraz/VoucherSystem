using VoucherSystem.API.DTOs;

namespace VoucherSystem.API.Messages;

public class VoucherMessages
{
    public record ListVouchersRequest();

    public record ListVouchersResponse(IEnumerable<VoucherDto> Vouchers);

    public record SelectVoucherRequest(int VoucherId, int Amount);

    public record SelectVoucherResponse(VoucherDto SelectedVoucher);

    public record AddToCartRequest(int VoucherId, int Amount);

    public record AddToCartResponse(bool Success);

    public record CheckoutRequest(int CartId);

    public record CheckoutResponse(bool Success);
}