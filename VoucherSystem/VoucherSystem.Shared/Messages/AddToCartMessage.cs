namespace VoucherSystem.Shared.Messages;

public class AddToCartMessage
{
    public record AddToCartRequest(int VoucherId, int Amount);

    public record AddToCartResponse(bool Success);
}