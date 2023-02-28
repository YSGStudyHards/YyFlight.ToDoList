using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Domain
{
    public abstract class EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
