using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Domain.System
{
    [Table("yyflight_todolist_updatelog")]
    public class UpdateLog : EntityBase
    {
        /// <summary>
        /// 系统更新内容
        /// </summary>

        public string UpdateContent { get; set; }
    }
}
