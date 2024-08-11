namespace VoucherSystem.Shared.Messages;

public class CheckoutMessage
{
    public record CheckoutRequest();

    public record CheckoutResponse(bool Success);
}