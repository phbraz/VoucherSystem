using Microsoft.AspNetCore.Mvc;
using VoucherSystem.API.DTOs;
using VoucherSystem.API.Models;
using VoucherSystem.API.Services.Interfaces;

namespace VoucherSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoucherController : ControllerBase
{
    private readonly IVoucherService _voucherService;

    public VoucherController(IVoucherService voucherService)
    {
        _voucherService = voucherService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VoucherDto>>> ListVouchers()
    {
        var vouchers = await _voucherService.ListVouchers();
        return Ok(vouchers);
    }

    [HttpPost("select")]
    public async Task<ActionResult<VoucherDto>> SelectVoucherAndAmount([FromBody] ApiModels.SelectVoucherRequest request)
    {
        var selectedVoucher = await _voucherService.SelectVoucherAndAmount(request.VoucherId, request.Amount);
        return Ok(selectedVoucher);
    }

    [HttpPost("addToCart")]
    public async Task<ActionResult> AddToCart([FromBody] ApiModels.AddToCartRequest request)
    {
        await _voucherService.AddToCart(request.VoucherId, request.Amount);
        return Ok();
    }

    [HttpPost("checkout")]
    public async Task<ActionResult> Checkout([FromBody] ApiModels.CheckoutRequest request)
    {
        await _voucherService.Checkout(request.CartId);
        return Ok();
    }
    
}