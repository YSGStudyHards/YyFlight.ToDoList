using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Repository.Interface;

namespace Repository
{
    public class MongoConnection : IMongoConnection
    {
        //基于MongoDB的最佳实践对于MongoClient最好设置为单例注入，因为在MongoDB.Driver中MongoClient已经被设计为线程安全可以被多线程共享，这样可还以避免反复实例化MongoClient带来的开销，避免在极端情况下的性能低下。
        //我们这里设计一个MongoConnection类，用于包裹这个MongoClient，然后将其以单例模式注入IoC容器中。
        public MongoClient MongoDBClient { get; set; }

        public IMongoDatabase DatabaseName { get; set; }

        public MongoConnection(IConfiguration configuration)
        {
            MongoDBClient = new MongoClient(configuration["MongoSettings:Connection"]);
            DatabaseName = MongoDBClient.GetDatabase(configuration["MongoSettings:DatabaseName"]);
        }
    }
}
