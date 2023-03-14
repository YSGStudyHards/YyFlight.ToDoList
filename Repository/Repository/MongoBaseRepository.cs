using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interface;

namespace Repository
{
    public class MongoBaseRepository<T> : IMongoRepository<T> where T : class, new()
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<T> _dbSet;

        protected MongoBaseRepository(IMongoContext context)
        {
            _context = context;
            _dbSet = _context.GetCollection<T>(typeof(T).Name);
        }

        #region 异步方法

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        public Task AddAsync(T objData)
        {
            return _context.AddCommandAsync(async () => await _dbSet.InsertOneAsync(objData));
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="objDatas">实体集合</param>
        /// <returns></returns>
        public Task InsertManyAsync(List<T> objDatas)
        {
            return _context.AddCommandAsync(async () => await _dbSet.InsertManyAsync(objDatas));
        }

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public Task DeleteAsync(string id)
        {
            return _context.AddCommandAsync(() => _dbSet.DeleteOneAsync(Builders<T>.Filter.Eq(" _id ", id)));
        }

        /// <summary>
        /// 数据修改
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="id">修改条件</param>
        /// <returns></returns>
        public Task UpdateAsync(T obj, string id)
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
            return _context.AddCommandAsync(async () =>
            {
                await _dbSet.UpdateOneAsync(filter, updatefilter);
            });
        }

        /// <summary>
        /// 通过ID主键获取数据
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(string id)
        {
            var data = await _dbSet.FindAsync(Builders<T>.Filter.Eq(" _id ", id));
            return data.FirstOrDefault();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var all = await _dbSet.FindAsync(Builders<T>.Filter.Empty);
            return all.ToList();
        }

        #endregion

        #region 同步方法

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        public Task Add(T objData)
        {
            return _context.AddCommandAsync(async () => await _dbSet.InsertOneAsync(objData));
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="objDatas">实体集合</param>
        /// <returns></returns>
        public Task InsertMany(List<T> objDatas)
        {
            return _context.AddCommandAsync(async () => await _dbSet.InsertManyAsync(objDatas));
        }

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public Task Delete(string id)
        {
            return _context.AddCommandAsync(() => _dbSet.DeleteOneAsync(Builders<T>.Filter.Eq(" _id ", id)));
        }

        /// <summary>
        /// 数据修改
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="id">修改条件</param>
        /// <returns></returns>
        public Task Update(T obj, string id)
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
            return _context.AddCommandAsync(async () =>
            {
                await _dbSet.UpdateOneAsync(filter, updatefilter);
            });
        }

        /// <summary>
        /// 通过ID主键获取数据
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task<T> GetById(string id)
        {
            var data = await _dbSet.FindAsync(Builders<T>.Filter.Eq(" _id ", id));
            return data.FirstOrDefault();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            var all = await _dbSet.FindAsync(Builders<T>.Filter.Empty);
            return all.ToList();
        }

        #endregion 
    }
}
