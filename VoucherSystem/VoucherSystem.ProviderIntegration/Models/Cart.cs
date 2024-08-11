using VoucherSystem.Shared.DTOs;
using VoucherSystem.Shared.Enums;

namespace VoucherSystem.ProviderIntegration.Models;

public class Cart
{
    public int UserId { get; set; }
    public CartStatus Status { get; set; }
    public IEnumerable<VoucherDto> Items { get; set; }
    public decimal Price { get; set; }
}