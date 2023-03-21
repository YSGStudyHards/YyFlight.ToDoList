using Application.User.ViewModel;
using Repository.Domain.User;
using Repository.Interface;

namespace Application.User
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _uow;
        private readonly IMongoRepository<UserInfo> _mongoRepository;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="mongoRepository">mongoRepository</param>
        /// <param name="uow">uow</param>
        public UserServices(IMongoRepository<UserInfo> mongoRepository, IUnitOfWork uow)
        {
            _uow = uow;
            _mongoRepository = mongoRepository;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserInfo>> GetAllUserInfos()
        {
            var getAllUserInfos = await _mongoRepository.GetAllAsync();
            return getAllUserInfos;
        }

        /// <summary>
        /// 通过主键用户ID获取用户信息
        /// </summary>
        /// <param name="id">主键用户ID</param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserInfoById(string id)
        {
            var userInfo = await _mongoRepository.GetByIdAsync(id);
            return userInfo;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo"></param>
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
            await _mongoRepository.AddAsync(addUserInfo);

            //查不到任何信息
            var testUserInfo = await _mongoRepository.GetByIdAsync(addUserInfo.Id);

            //提交新增用户信息操作
            await _uow.Commit();

            //UserInfo只有在提交后才会被添加
            testUserInfo = await _mongoRepository.GetByIdAsync(addUserInfo.Id);

            return testUserInfo;
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userInfo"></param>
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

            await _mongoRepository.UpdateAsync(updateUserInfo, id);

            await _uow.Commit();

            return await _mongoRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// 用户信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            await _mongoRepository.DeleteAsync(id);

            //任然可以查询到用户信息
            var testUserInfo = await _mongoRepository.GetByIdAsync(id);

            //提交用户删除操作
            await _uow.Commit();

            //已经查询不到该用户信息了
            testUserInfo = await _mongoRepository.GetByIdAsync(id);

            return testUserInfo == null;
        }
    }
}
