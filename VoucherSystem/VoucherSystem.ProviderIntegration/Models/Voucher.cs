using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VoucherSystem.ProviderIntegration.Models;

public class Voucher
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
    public decimal Value { get; set; }
    public int Amount { get; set; }
}