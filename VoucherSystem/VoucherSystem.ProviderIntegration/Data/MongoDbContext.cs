using MongoDB.Driver;
using VoucherSystem.ProviderIntegration.Configuration;
using VoucherSystem.ProviderIntegration.Models;

namespace VoucherSystem.ProviderIntegration.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<Voucher> Vouchers => _database.GetCollection<Voucher>("Vouchers");
}