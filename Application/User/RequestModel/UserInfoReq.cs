namespace Application.User.RequestModel
{
    public class UserInfoReq
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>

        public string Password { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadPortrait { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户状态（0冻结，1正常，2注销）
        /// </summary>
        public int Status { get; set; }
    }
}
