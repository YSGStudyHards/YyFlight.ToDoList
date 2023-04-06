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
        public IClientSessionHandle? Session = null;

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
        /// 保存更改
        /// TODO：MongoDB单机服务器不支持事务【使用MongoDB事务会报错：Standalone servers do not support transactions】,只有在集群情况下才支持事务
        /// 原因：MongoDB在使用分布式事务时需要进行多节点之间的协调和通信，而单机环境下无法实现这样的分布式协调和通信机制。但是，在MongoDB部署为一个集群（cluster）后，将多个计算机连接为一个整体，通过协调和通信机制实现了分布式事务的正常使用。从数据一致性和可靠性的角度来看，在分布式系统中实现事务处理是至关重要的。而在单机环境下不支持事务，只有在集群情况下才支持事务的设计方式是为了保证数据一致性和可靠性，并且也符合分布式系统的设计思想。
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            using (Session = await _mongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }
            return _commands.Count;
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
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
