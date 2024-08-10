using MongoDB.Driver;
using VoucherSystem.ProviderIntegration.DTOs;
using VoucherSystem.ProviderIntegration.Models;

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
                new VoucherDto { Name = "10% Off", Value = 10, Amount = 100 },
                new VoucherDto { Name = "20% Off", Value = 20, Amount = 50 },
                new VoucherDto { Name = "5 EUR Off", Value = 5, Amount = 200 }
            };
            await collection.InsertManyAsync(vouchers);
        }
    }
}