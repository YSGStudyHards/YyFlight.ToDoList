using MongoDB.Driver;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IMongoRepository<T> where T : class, new()
    {
        #region 事务操作示例

        /// <summary>
        /// 事务添加数据
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        Task AddTransactionsAsync(IClientSessionHandle session, T objData);

        /// <summary>
        /// 事务数据删除
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        Task DeleteTransactionsAsync(IClientSessionHandle session, string id);

        /// <summary>
        /// 事务异步局部更新（仅更新一条记录）
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新条件</param>
        /// <returns></returns>
        Task UpdateTransactionsAsync(IClientSessionHandle session, FilterDefinition<T> filter, UpdateDefinition<T> update);

        #endregion

        #region 添加相关操作

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

        #endregion

        #region 删除相关操作

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        Task DeleteAsync(string id);

        /// <summary>
        /// 异步删除多条数据
        /// </summary>
        /// <param name="filter">删除的条件</param>
        /// <returns></returns>
        Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter);

        #endregion

        #region 修改相关操作

        /// <summary>
        /// 指定对象异步修改一条数据
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="id">修改条件</param>
        /// <returns></returns>
        Task UpdateAsync(T obj, string id);

        /// <summary>
        /// 局部更新（仅更新一条记录）
        /// <para><![CDATA[expression 参数示例：x => x.Id == 1 && x.Age > 18 && x.Gender == 0]]></para>
        /// <para><![CDATA[entity 参数示例：y => new T{ RealName = "Ray", Gender = 1}]]></para>
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <param name="entity">更新条件</param>
        /// <returns></returns>
        Task UpdateAsync(Expression<Func<T, bool>> expression, Expression<Action<T>> entity);

        /// <summary>
        /// 异步局部更新（仅更新一条记录）
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新条件</param>
        /// <returns></returns>
        Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);

        /// <summary>
        /// 异步局部更新（仅更新多条记录）
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <param name="update">更新条件</param>
        /// <returns></returns>
        Task UpdateManyAsync(Expression<Func<T, bool>> expression, UpdateDefinition<T> update);

        /// <summary>
        /// 异步批量修改数据
        /// </summary>
        /// <param name="dic">要修改的字段</param>
        /// <param name="filter">更新条件</param>
        /// <returns></returns>
        Task<UpdateResult> UpdateManayAsync(Dictionary<string, string> dic, FilterDefinition<T> filter);

        #endregion

        #region 查询统计相关操作

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

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <returns></returns>
        Task<long> CountAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        Task<long> CountAsync(FilterDefinition<T> filter);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 异步查询集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        Task<List<T>> FindListAsync(FilterDefinition<T> filter, string[]? field = null, SortDefinition<T>? sort = null);

        /// <summary>
        /// 异步分页查询集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        Task<List<T>> FindListByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, string[]? field = null, SortDefinition<T>? sort = null);

        #endregion
    }
}
