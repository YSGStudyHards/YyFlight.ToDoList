using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Repository.Interface;

namespace Repository
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase _database;
        private MongoClient _mongoClient;
        private readonly IConfiguration _configuration;
        private readonly List<Func<Task>> _commands;

        public MongoContext(IConfiguration configuration)
        {
            _configuration = configuration;
            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Func<Task>>();
            // Configure mongo (You can inject the config, just to simplify)
            _mongoClient = new MongoClient(_configuration["MongoSettings:Connection"]);
            _database = _mongoClient.GetDatabase(_configuration["MongoSettings:DatabaseName"]);
        }

        /// <summary>
        /// 添加命令操作
        /// </summary>
        /// <param name="func">委托</param>
        /// <returns></returns>
        public async Task AddCommandAsync(Func<Task> func)
        {
            _commands.Add(func);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取MongoDB集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">集合名称</param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        /// <summary>
        /// 释放上下文
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
