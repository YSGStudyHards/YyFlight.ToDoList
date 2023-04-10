using MongoDB.Driver;
using Repository.Interface;

namespace Repository
{
    /// <summary>
    /// 工作单元类
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 提交保存更改
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <returns></returns>
        public async Task<bool> Commit(IClientSessionHandle session)
        {
            return await _context.SaveChangesAsync(session) > 0;
        }

        /// <summary>
        /// 初始化MongoDB会话对象session
        /// </summary>
        /// <returns></returns>
        public async Task<IClientSessionHandle> InitTransaction()
        {
            return await _context.StartSessionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
