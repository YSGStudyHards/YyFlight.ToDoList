using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Domain.TodoTask
{
    [Table("yyflight_todolist_content")]
    public class TodoListContent: EntityBase
    {
        /// <summary>
        /// 用户主键ID
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserID { get; set; }

        /// <summary>
        /// 待办清单内容
        /// </summary>

        public string Content { get; set; }

        /// <summary>
        /// 待办清单过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// 是否需要提醒
        /// </summary>
        public bool Isremind { get; set; }

        /// <summary>
        /// 过期前多少个小时提醒
        /// </summary>
        public int RemindTime { get; set; }

        /// <summary>
        /// 完成状态（0待完成，1已完成）
        /// </summary>
        public int CompleteStatus { get; set; }
    }
}
