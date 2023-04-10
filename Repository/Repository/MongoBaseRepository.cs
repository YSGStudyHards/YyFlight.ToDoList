using Infrastructure.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Repository
{
    public class MongoBaseRepository<T> : IMongoRepository<T> where T : class, new()
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<T> _dbSet;
        private readonly string _collectionName;

        protected MongoBaseRepository(IMongoContext context)
        {
            _context = context;
            _collectionName = typeof(T).GetAttributeValue((TableAttribute m) => m.Name) ?? typeof(T).Name;
            _dbSet = _context.GetCollection<T>(_collectionName);
        }

        #region 事务操作示例

        /// <summary>
        /// 事务添加数据
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        public async Task AddTransactionsAsync(IClientSessionHandle session, T objData)
        {
            await _context.AddCommandAsync(async (session) => await _dbSet.InsertOneAsync(objData));
        }

        /// <summary>
        /// 事务数据删除
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task DeleteTransactionsAsync(IClientSessionHandle session, string id)
        {
            await _context.AddCommandAsync((session) => _dbSet.DeleteOneAsync(Builders<T>.Filter.Eq(" _id ", id)));
        }

        /// <summary>
        /// 事务异步局部更新（仅更新一条记录）
        /// </summary>
        /// <param name="session">MongoDB 会话（session）对象</param>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新条件</param>
        /// <returns></returns>
        public async Task UpdateTransactionsAsync(IClientSessionHandle session, FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            await _context.AddCommandAsync((session) => _dbSet.UpdateOneAsync(filter, update));
        }

        #endregion

        #region 添加相关操作

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        public async Task AddAsync(T objData)
        {
            await _dbSet.InsertOneAsync(objData);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="objDatas">实体集合</param>
        /// <returns></returns>
        public async Task InsertManyAsync(List<T> objDatas)
        {
            await _dbSet.InsertManyAsync(objDatas);
        }

        #endregion

        #region 删除相关操作

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            await _dbSet.DeleteOneAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)));
        }

        /// <summary>
        /// 异步删除多条数据
        /// </summary>
        /// <param name="filter">删除的条件</param>
        /// <returns></returns>
        public async Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter)
        {
            return await _dbSet.DeleteManyAsync(filter);
        }

        #endregion

        #region 修改相关操作

        /// <summary>
        /// 指定对象异步修改一条数据
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="id">修改条件</param>
        /// <returns></returns>
        public async Task UpdateAsync(T obj, string id)
        {
            //修改条件
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            //要修改的字段
            var list = new List<UpdateDefinition<T>>();
            foreach (var item in obj.GetType().GetProperties())
            {
                if (item.Name.ToLower() == "id") continue;
                list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(obj)));
            }
            var updatefilter = Builders<T>.Update.Combine(list);
            await _dbSet.UpdateOneAsync(filter, updatefilter);
        }

        /// <summary>
        /// 局部更新（仅更新一条记录）
        /// <para><![CDATA[expression 参数示例：x => x.Id == 1 && x.Age > 18 && x.Gender == 0]]></para>
        /// <para><![CDATA[entity 参数示例：y => new T{ RealName = "Ray", Gender = 1}]]></para>
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <param name="entity">更新条件</param>
        /// <returns></returns>
        public async Task UpdateAsync(Expression<Func<T, bool>> expression, Expression<Action<T>> entity)
        {
            var fieldList = new List<UpdateDefinition<T>>();

            if (entity.Body is MemberInitExpression param)
            {
                foreach (var item in param.Bindings)
                {
                    var propertyName = item.Member.Name;
                    object propertyValue = null;

                    if (item is not MemberAssignment memberAssignment) continue;

                    if (memberAssignment.Expression.NodeType == ExpressionType.Constant)
                    {
                        if (memberAssignment.Expression is ConstantExpression constantExpression)
                            propertyValue = constantExpression.Value;
                    }
                    else
                    {
                        propertyValue = Expression.Lambda(memberAssignment.Expression, null).Compile().DynamicInvoke();
                    }

                    if (propertyName != "_id") //实体键_id不允许更新
                    {
                        fieldList.Add(Builders<T>.Update.Set(propertyName, propertyValue));
                    }
                }
            }

            await _dbSet.UpdateOneAsync(expression, Builders<T>.Update.Combine(fieldList));
        }

        /// <summary>
        /// 异步局部更新（仅更新一条记录）
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新条件</param>
        /// <returns></returns>
        public async Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            await _dbSet.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// 异步局部更新（仅更新多条记录）
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <param name="update">更新条件</param>
        /// <returns></returns>
        public async Task UpdateManyAsync(Expression<Func<T, bool>> expression, UpdateDefinition<T> update)
        {
            await _dbSet.UpdateManyAsync(expression, update);
        }

        /// <summary>
        /// 异步批量修改数据
        /// </summary>
        /// <param name="dic">要修改的字段</param>
        /// <param name="filter">更新条件</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateManayAsync(Dictionary<string, string> dic, FilterDefinition<T> filter)
        {
            T t = new T();
            //要修改的字段
            var list = new List<UpdateDefinition<T>>();
            foreach (var item in t.GetType().GetProperties())
            {
                if (!dic.ContainsKey(item.Name)) continue;
                var value = dic[item.Name];
                list.Add(Builders<T>.Update.Set(item.Name, value));
            }
            var updatefilter = Builders<T>.Update.Combine(list);
            return await _dbSet.UpdateManyAsync(filter, updatefilter);
        }

        #endregion

        #region 查询统计相关操作

        /// <summary>
        /// 通过ID主键获取数据
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(string id)
        {
            var queryData = await _dbSet.FindAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)));
            return queryData.FirstOrDefault();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var queryAllData = await _dbSet.FindAsync(Builders<T>.Filter.Empty);
            return queryAllData.ToList();
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <returns></returns>
        public async Task<long> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.CountDocumentsAsync(expression);
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <returns></returns>
        public async Task<long> CountAsync(FilterDefinition<T> filter)
        {
            return await _dbSet.CountDocumentsAsync(filter);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(_dbSet.AsQueryable().Any(predicate));
        }

        /// <summary>
        /// 异步查询集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListAsync(FilterDefinition<T> filter, string[]? field = null, SortDefinition<T>? sort = null)
        {
            //不指定查询字段
            if (field == null || field.Length == 0)
            {
                if (sort == null) return await _dbSet.Find(filter).ToListAsync();
                return await _dbSet.Find(filter).Sort(sort).ToListAsync();
            }

            //指定查询字段
            var fieldList = new List<ProjectionDefinition<T>>();
            for (int i = 0; i < field.Length; i++)
            {
                fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
            }
            var projection = Builders<T>.Projection.Combine(fieldList);
            fieldList?.Clear();

            //不排序
            if (sort == null) return await _dbSet.Find(filter).Project<T>(projection).ToListAsync();

            //排序查询
            return await _dbSet.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
        }

        /// <summary>
        /// 异步分页查询集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="field">要查询的字段,不写时查询全部</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public async Task<List<T>> FindListByPageAsync(FilterDefinition<T> filter, int pageIndex, int pageSize, string[]? field = null, SortDefinition<T>? sort = null)
        {
            //不指定查询字段
            if (field == null || field.Length == 0)
            {
                if (sort == null) return await _dbSet.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                //进行排序
                return await _dbSet.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
            }

            //指定查询字段
            var fieldList = new List<ProjectionDefinition<T>>();
            for (int i = 0; i < field.Length; i++)
            {
                fieldList.Add(Builders<T>.Projection.Include(field[i].ToString()));
            }
            var projection = Builders<T>.Projection.Combine(fieldList);
            fieldList?.Clear();

            //不排序
            if (sort == null) return await _dbSet.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();

            //排序查询
            return await _dbSet.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        #endregion
    }
}
