using Application.User.ViewModel;
using Repository.Domain.User;
using Repository.Interface;
using Repository.Repositories.User;

namespace Application.User
{
    public class UserOperationExampleServices : IUserOperationExampleServices
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="userRepository">userRepository</param>
        /// <param name="uow">uow</param>
        public UserOperationExampleServices(IUserRepository userRepository, IUnitOfWork uow)
        {
            _uow = uow;
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
            var userInfo = await _userRepository.GetByIdAsync(id);
            return userInfo;
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

            //查不到任何信息
            var testUserInfo = await _userRepository.GetByIdAsync(addUserInfo.Id);

            //提交新增用户信息操作
            await _uow.Commit();

            //UserInfo只有在提交后才会被添加
            testUserInfo = await _userRepository.GetByIdAsync(addUserInfo.Id);

            return testUserInfo;
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

            await _uow.Commit();

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

            //任然可以查询到用户信息
            var testUserInfo = await _userRepository.GetByIdAsync(id);

            //提交用户删除操作
            await _uow.Commit();

            //已经查询不到该用户信息了
            testUserInfo = await _userRepository.GetByIdAsync(id);

            return testUserInfo == null;
        }
    }
}
