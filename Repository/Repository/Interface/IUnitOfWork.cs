using MongoDB.Driver;

namespace Repository.Interface
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 提交保存更改
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <returns></returns>
        Task<bool> Commit(IClientSessionHandle session);

        /// <summary>
        /// 初始化MongoDB会话对象session
        /// </summary>
        /// <returns></returns>
        Task<IClientSessionHandle> InitTransaction();
    }
}
