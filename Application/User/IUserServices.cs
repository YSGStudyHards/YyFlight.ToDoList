using Application.User.ViewModel;
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
        /// 通过主键用户ID获取用户信息
        /// </summary>
        /// <param name="id">主键用户ID</param>
        /// <returns></returns>
        Task<UserInfo> GetUserInfoById(string id);

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<UserInfo> AddUserInfo(UserInfoViewModel userInfo);

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        Task<UserInfo> UpdateUserInfo(string id, UserInfoViewModel userInfo);

        /// <summary>
        /// 用户信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(string id);
    }
}
