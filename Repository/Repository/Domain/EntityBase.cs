using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Domain
{
    public class EntityBase
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}
