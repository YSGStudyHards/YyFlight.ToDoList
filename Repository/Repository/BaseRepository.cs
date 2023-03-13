using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Interface;

namespace Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class, new()
    {
        protected readonly IMongoContext _context;
        protected readonly IMongoCollection<T> DbSet;

        protected BaseRepository(IMongoContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="objData">添加数据</param>
        /// <returns></returns>
        public Task Add(T objData)
        {
            return _context.AddCommand(async () => await DbSet.InsertOneAsync(objData));
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="objDatas">实体集合</param>
        /// <returns></returns>
        public Task InsertMany(List<T> objDatas)
        {
            return _context.AddCommand(async () => await DbSet.InsertManyAsync(objDatas));
        }

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public Task Delete(string id)
        {
            return _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<T>.Filter.Eq(" _id ", id)));
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
            return _context.AddCommand(async () =>
            {
                await DbSet.UpdateOneAsync(filter, updatefilter);
            });
        }

        /// <summary>
        /// 通过ID主键获取数据
        /// </summary>
        /// <param name="id">objectId</param>
        /// <returns></returns>
        public async Task<T> GetById(string id)
        {
            var data = await DbSet.FindAsync(Builders<T>.Filter.Eq(" _id ", id));
            return data.FirstOrDefault();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<T>.Filter.Empty);
            return all.ToList();
        }

        ///// <summary>
        ///// 异步根据条件获取总数
        ///// </summary>
        ///// <param name="host">mongodb连接信息</param>
        ///// <param name="filter">条件</param>
        ///// <returns></returns>
        //public static async Task<long> CountAsync(FilterDefinition<T> filter)
        //{


        //    //try
        //    //{
        //    //    var client = MongodbClient<T>.MongodbInfoClient(host);
        //    //    return await client.CountAsync(filter);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex;
        //    //}
        //}
    }
}
