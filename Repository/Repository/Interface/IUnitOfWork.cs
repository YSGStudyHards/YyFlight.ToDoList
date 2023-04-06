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
        /// <returns></returns>
        Task<bool> Commit();
    }
}
