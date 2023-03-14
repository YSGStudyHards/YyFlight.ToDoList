namespace Repository.Interface
{
    public interface IMongoRepository<T> where T : class, new()
    {
        #region 异步方法

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        Task AddAsync(T objData);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="objDatas">实体集合</param>
        /// <returns></returns>
        Task InsertManyAsync(List<T> objDatas);

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        Task DeleteAsync(string id);

        /// <summary>
        /// 数据修改
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="id">修改条件</param>
        /// <returns></returns>
        Task UpdateAsync(T obj, string id);

        /// <summary>
        /// 通过ID主键获取数据
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        #endregion

        #region 同步方法

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        void Add(T objData);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="objDatas">实体集合</param>
        /// <returns></returns>
        void InsertMany(List<T> objDatas);

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        void Delete(string id);

        /// <summary>
        /// 数据修改
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="id">修改条件</param>
        /// <returns></returns>
        void Update(T obj, string id);

        /// <summary>
        /// 通过ID主键获取数据
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        T GetById(string id);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        #endregion
    }
}
