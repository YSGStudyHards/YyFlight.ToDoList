namespace Application.User.RequestModel
{
    public class UserInfoByPageListReq
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int PageSize { get; set; }
    }
}
