﻿namespace VoucherSystem.API.Models;

public class ApiModels
{
    public class ListVouchersRequest { }

    public class SelectVoucherRequest
    {
        public int VoucherId { get; set; }
        public int Amount { get; set; }
    }

    public class CheckoutRequest
    {
    }
}