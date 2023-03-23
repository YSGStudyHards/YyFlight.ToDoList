using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Repository.Persistence
{
    public static class MongoDbPersistence
    {
        /// <summary>
        /// MongoDB持久化配置
        /// </summary>
        public static void Configure()
        {
            //在MongoDB中存储和检索Guid数据时，需要将Guid转换为Bson类型，因此使用BsonSerializer.RegisterSerializer方法将GuidSerializer注册为一个自定义序列化器，来实现Guid数据的序列化和反序列化。
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }
    }
}
