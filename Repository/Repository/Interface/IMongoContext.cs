using MongoDB.Driver;

namespace Repository.Interface
{
    public interface IMongoContext : IDisposable
    {
        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
