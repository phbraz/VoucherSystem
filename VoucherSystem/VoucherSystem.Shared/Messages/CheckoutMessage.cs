namespace VoucherSystem.Shared.Messages;

public class CheckoutMessage
{
    public record CheckoutRequest(int CartId);

    public record CheckoutResponse(bool Success);
}