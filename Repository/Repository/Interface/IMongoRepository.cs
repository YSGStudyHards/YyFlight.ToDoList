using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

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

        #endregion
    }
}
