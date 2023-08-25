using MongoDB.Driver;
using Repository.Interface;

namespace Repository
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _databaseName;
        private readonly MongoClient _mongoClient;
        //这里将 _commands 中的每个元素都定义为一个 Func<IClientSessionHandle, Task> 委托，此委托表示一个需要 IClientSessionHandle 对象作为参数并返回一个异步任务的方法
        //每个委托都表示一个MongoDB 会话（session）对象和要执行的命令
        private readonly List<Func<IClientSessionHandle, Task>> _commands = new List<Func<IClientSessionHandle, Task>>();

        public MongoContext(IMongoConnection mongoConnection)
        {
            _mongoClient = mongoConnection.MongoDBClient;
            _databaseName = mongoConnection.DatabaseName;
        }

        /// <summary>
        /// 添加命令操作
        /// </summary>
        /// <param name="func">方法接受一个 Func<IClientSessionHandle, Task> 委托作为参数，该委托表示一个需要 IClientSessionHandle 对象作为参数并返回一个异步任务的方法</param>
        /// <returns></returns>
        public async Task AddCommandAsync(Func<IClientSessionHandle, Task> func)
        {
            _commands.Add(func);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 提交更改并返回受影响的行数
        /// TODO：MongoDB单机服务器不支持事务【使用MongoDB事务会报错：Standalone servers do not support transactions】,只有在集群情况下才支持事务【后面需要搭建集群环境测试】
        /// 原因：MongoDB在使用分布式事务时需要进行多节点之间的协调和通信，而单机环境下无法实现这样的分布式协调和通信机制。但是，在MongoDB部署为一个集群（cluster）后，将多个计算机连接为一个整体，通过协调和通信机制实现了分布式事务的正常使用。从数据一致性和可靠性的角度来看，在分布式系统中实现事务处理是至关重要的。而在单机环境下不支持事务，只有在集群情况下才支持事务的设计方式是为了保证数据一致性和可靠性，并且也符合分布式系统的设计思想。
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(IClientSessionHandle session)
        {
            try
            {
                session.StartTransaction();//开始事务

                foreach (var command in _commands)
                {
                    await command(session);
                    //语句实现了对事务中所有操作的异步执行，并等待它们完成。如果没有错误发生，程序会继续执行session.CommitTransactionAsync();方法，将之前进行的所有更改一起提交到MongoDB服务器上，从而实现事务提交。
                }

                await session.CommitTransactionAsync();//提交事务
                return _commands.Count;
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();//回滚事务
                return 0;
            }
            finally
            {
                _commands.Clear();//清空_commands列表中的元素
            }
        }

        /// <summary>
        /// 初始化Mongodb会话对象session
        /// </summary>
        /// <returns></returns>
        public async Task<IClientSessionHandle> StartSessionAsync()
        {
            var session = await _mongoClient.StartSessionAsync();
            return session;
        }

        /// <summary>
        /// 获取MongoDB集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">集合名称</param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _databaseName.GetCollection<T>(name);
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
