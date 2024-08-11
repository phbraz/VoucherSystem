using Microsoft.AspNetCore.Mvc;
using VoucherSystem.API.Models;
using VoucherSystem.API.Services.Interfaces;
using VoucherSystem.Shared.DTOs;

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

    [HttpPost("checkout")]
    public async Task<ActionResult> Checkout([FromBody] ApiModels.CheckoutRequest request)
    {
        var isCheckoutComplete = await _voucherService.Checkout();
        return isCheckoutComplete ? Ok() : BadRequest("Your cart might be empty or something went wrong");
    }
    
}