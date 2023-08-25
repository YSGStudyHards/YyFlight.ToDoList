using MongoDB.Driver;

namespace Repository.Interface
{
    public interface IMongoContext : IDisposable
    {
        /// <summary>
        /// 添加命令操作
        /// </summary>
        /// <param name="func">委托 方法接受一个 Func<IClientSessionHandle, Task> 委托作为参数，该委托表示一个需要 IClientSessionHandle 对象作为参数并返回一个异步任务的方法</param>
        /// <returns></returns>
        Task AddCommandAsync(Func<IClientSessionHandle, Task> func);

        /// <summary>
        /// 提交更改并返回受影响的行数
        /// TODO：MongoDB单机服务器不支持事务【使用MongoDB事务会报错：Standalone servers do not support transactions】,只有在集群情况下才支持事务【后面需要搭建集群环境测试】
        /// 原因：MongoDB在使用分布式事务时需要进行多节点之间的协调和通信，而单机环境下无法实现这样的分布式协调和通信机制。但是，在MongoDB部署为一个集群（cluster）后，将多个计算机连接为一个整体，通过协调和通信机制实现了分布式事务的正常使用。从数据一致性和可靠性的角度来看，在分布式系统中实现事务处理是至关重要的。而在单机环境下不支持事务，只有在集群情况下才支持事务的设计方式是为了保证数据一致性和可靠性，并且也符合分布式系统的设计思想。
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(IClientSessionHandle session);

        /// <summary>
        /// 初始化Mongodb会话对象session
        /// </summary>
        /// <returns></returns>
        Task<IClientSessionHandle> StartSessionAsync();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
