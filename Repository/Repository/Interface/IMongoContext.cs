using MongoDB.Driver;
using System;

namespace Repository.Interface
{
    public interface IMongoContext
    {
        /// <summary>
        /// 添加命令操作
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        Task AddCommandAsync(Func<Task> func);

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 获取集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IMongoCollection<T> GetCollection<T>(string name);

        /// <summary>
        /// 释放上下文
        /// </summary>
        void Dispose();
    }
}
