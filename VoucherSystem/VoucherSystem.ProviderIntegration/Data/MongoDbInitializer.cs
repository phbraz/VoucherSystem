using MongoDB.Driver;
using VoucherSystem.ProviderIntegration.Models;
using VoucherSystem.Shared.DTOs;
using VoucherSystem.Shared.Enums;

namespace VoucherSystem.ProviderIntegration.Data;

public static class MongoDbInitializer
{
    public static async Task SeedData(IMongoDatabase database)
    {
        var collection = database.GetCollection<VoucherDto>("Vouchers");
        if (await collection.CountDocumentsAsync(FilterDefinition<VoucherDto>.Empty) == 0)
        {
            var vouchers = new List<VoucherDto>
            {
                new VoucherDto { Id = 1, Name = "10% Off", Value = 10, Amount = 100 },
                new VoucherDto { Id = 2, Name = "20% Off", Value = 20, Amount = 50 },
                new VoucherDto { Id = 3, Name = "5% off", Value = 5, Amount = 200 }
            };
            await collection.InsertManyAsync(vouchers);
        }
    }
}