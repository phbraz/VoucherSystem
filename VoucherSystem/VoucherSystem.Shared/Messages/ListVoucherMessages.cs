using VoucherSystem.Shared.DTOs;

namespace VoucherSystem.Shared.Messages;

public class ListVoucherMessages
{
    public record ListVouchersRequest();

    public record ListVouchersResponse(IEnumerable<VoucherDto> Vouchers);
}