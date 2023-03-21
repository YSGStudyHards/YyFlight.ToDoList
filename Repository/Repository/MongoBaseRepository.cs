using Infrastructure.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interface;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Security.Cryptography;

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

        #region 异步方法

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        public async Task AddAsync(T objData)
        {
            await _context.AddCommandAsync(async () => await _dbSet.InsertOneAsync(objData));
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="objDatas">实体集合</param>
        /// <returns></returns>
        public async Task InsertManyAsync(List<T> objDatas)
        {
           await _context.AddCommandAsync(async () => await _dbSet.InsertManyAsync(objDatas));
        }

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            await _context.AddCommandAsync(() => _dbSet.DeleteOneAsync(Builders<T>.Filter.Eq(" _id ", id)));
        }

        /// <summary>
        /// 数据修改
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
            await _context.AddCommandAsync(async () =>
            {
                await _dbSet.UpdateOneAsync(filter, updatefilter);
            });
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

            await _context.AddCommandAsync(async () =>
            {
              await _dbSet.UpdateOneAsync(expression, Builders<T>.Update.Combine(fieldList));
            });
        }

        /// <summary>
        /// 异步局部更新（仅更新一条记录）
        /// </summary>
        /// <param name="filter">过滤器</param>
        /// <param name="update">更新条件</param>
        /// <returns></returns>
        public async Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            await _context.AddCommandAsync(async () =>
            {
                await _dbSet.UpdateOneAsync(filter, update);
            });
        }

        /// <summary>
        /// 异步局部更新（仅更新多条记录）
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <param name="update">更新条件</param>
        /// <returns></returns>
        public async Task UpdateManyAsync(Expression<Func<T, bool>> expression,
            UpdateDefinition<T> update)
        {
            await _context.AddCommandAsync(async () =>
            {
                await _dbSet.UpdateManyAsync(expression, update);
            });
        }

        /// <summary>
        /// 通过ID主键获取数据
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(string id)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq(" _id ", ObjectId.Parse(id));
            var queryData =_dbSet.Find(filter/*Builders<T>.Filter.Eq(" _id ", new ObjectId(id))*/);
            var ttt= queryData.ToList();
            return await queryData.FirstOrDefaultAsync();
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

        #endregion
    }
}
