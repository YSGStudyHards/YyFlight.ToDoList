using Application.User.RequestModel;
using Repository.Domain.User;

namespace Application.User
{
    public interface IUserServices
    {
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserInfo>> GetAllUserInfos();

        /// <summary>
        /// 通过用户ID获取对应用户信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<UserInfo> GetUserInfoById(string id);

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        Task<bool> AddUserInfo(UserInfoReq userInfo);

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        Task<UserInfo> UpdateUserInfo(string id, UserInfoReq userInfo);

        /// <summary>
        /// 用户信息删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<bool> Delete(string id);
    }
}
