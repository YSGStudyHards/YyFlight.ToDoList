using Application.User.ViewModel;
using MongoDB.Bson;
using Repository.Domain.User;
using Repository.Repositories.User;

namespace Application.User
{
    public class UserOperationExampleServices : IUserOperationExampleServices
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="userRepository">userRepository</param>
        public UserOperationExampleServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserInfo>> GetAllUserInfos()
        {
            var getAllUserInfos = await _userRepository.GetAllAsync();
            return getAllUserInfos;
        }

        /// <summary>
        /// 通过用户ID获取对应用户信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserInfoById(string id)
        {
            var getUserInfo = await _userRepository.GetByIdAsync(id);
            return getUserInfo;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        public async Task<UserInfo> AddUserInfo(UserInfoViewModel userInfo)
        {
            var addUserInfo = new UserInfo()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                NickName = userInfo.NickName,
                Password = userInfo.Password,
                Status = 1,
                HeadPortrait = userInfo.HeadPortrait,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
            await _userRepository.AddAsync(addUserInfo);
            var queryUserInfo = await _userRepository.GetByIdAsync(addUserInfo.Id);
            return queryUserInfo;
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        public async Task<UserInfo> UpdateUserInfo(string id, UserInfoViewModel userInfo)
        {
            var updateUserInfo = new UserInfo()
            {
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                NickName = userInfo.NickName,
                Password = userInfo.Password,
                Status = 1,
                HeadPortrait = userInfo.HeadPortrait,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
            await _userRepository.UpdateAsync(updateUserInfo, id);
            return await _userRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// 用户信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            await _userRepository.DeleteAsync(id);
            var testUserInfo = await _userRepository.GetByIdAsync(id);
            return testUserInfo == null;
        }
    }
}
