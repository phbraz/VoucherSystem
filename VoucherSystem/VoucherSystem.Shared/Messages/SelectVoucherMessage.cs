using VoucherSystem.Shared.DTOs;

namespace VoucherSystem.Shared.Messages;

public class SelectVoucherMessage
{
    public record SelectVoucherRequest(int VoucherId, int Amount);

    public record SelectVoucherResponse(VoucherDto SelectedVoucher);
}