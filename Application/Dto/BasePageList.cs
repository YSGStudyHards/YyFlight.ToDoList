namespace Application.Dto
{
    public class BasePageList
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页码
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
