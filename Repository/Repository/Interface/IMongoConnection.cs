using MongoDB.Driver;

namespace Repository.Interface
{
    public interface IMongoConnection
    {
        public MongoClient MongoDBClient { get; set; }
        public IMongoDatabase DatabaseName { get; set; }
    }
}
