using Application.User.RequestModel;
using Infrastructure.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Domain.User;
using Repository.Interface;
using Repository.Repositories.User;

namespace Application.User
{
    public class UserOperationExampleServices : IUserOperationExampleServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>
        /// <param name="userRepository">userRepository</param>
        public UserOperationExampleServices(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
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
        /// 用户分页数据获取
        /// </summary>
        /// <param name="userInfoByPageListReq">userInfoByPageListReq</param>
        /// <returns></returns>
        public async Task<IEnumerable<UserInfo>> GetUserInfoByPageList(UserInfoByPageListReq request)
        {
            //创建查询条件构造器
            FilterDefinitionBuilder<UserInfo> buildFilter = Builders<UserInfo>.Filter;
            FilterDefinition<UserInfo> filter = buildFilter.Empty;
            SortDefinition<UserInfo> sort = Builders<UserInfo>.Sort.Ascending(m => m.CreateDate);
            if (!string.IsNullOrEmpty(request.NickName))
            {
                filter = buildFilter.Eq(m => m.NickName, request.NickName);
            }

            if (!string.IsNullOrEmpty(request.Id))
            {
                filter = buildFilter.Eq(m => m.Id, request.Id);
            }

            var list = await _userRepository.FindListByPageAsync(filter, request.PageIndex, request.PageSize, Array.Empty<string>(), sort);
            return list;
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
        public async Task<UserInfo> AddUserInfo(UserInfoReq userInfo)
        {
            var addUserInfo = new UserInfo()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                NickName = userInfo.NickName,
                Password = MD5Helper.MDString(userInfo.Password),
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
        /// 事务添加用户信息
        /// </summary>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        public async Task<UserInfo> AddUserInfoTransactions(UserInfoReq userInfo)
        {
            using var session = await _unitOfWork.InitTransaction();
            var addUserInfo = new UserInfo()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                NickName = userInfo.NickName,
                Password = MD5Helper.MDString(userInfo.Password),
                Status = 1,
                HeadPortrait = userInfo.HeadPortrait,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
            await _userRepository.AddTransactionsAsync(session, addUserInfo);

            //查不到任何信息
            var queryUserInfo = await _userRepository.GetByIdAsync(addUserInfo.Id);

            //提交新增用户信息操作
            await _unitOfWork.Commit(session);

            //UserInfo只有在提交后才会被添加
            queryUserInfo = await _userRepository.GetByIdAsync(addUserInfo.Id);

            return queryUserInfo;
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        public async Task<UserInfo> UpdateUserInfo(string id, UserInfoReq userInfo)
        {
            #region 指定字段和条件修改

            //修改条件
            var list = new List<FilterDefinition<UserInfo>>
            {
                Builders<UserInfo>.Filter.Eq("_id", new ObjectId(id))
            };
            var filter = Builders<UserInfo>.Filter.And(list);

            //指定要修改的字段内容
            //参考文章：https://chsakell.gitbook.io/mongodb-csharp-docs/crud-basics/update-documents
            var updateDefinition = Builders<UserInfo>.Update.
                Set(u => u.HeadPortrait, userInfo.HeadPortrait).
                Set(u => u.NickName, userInfo.NickName).
                Set(u => u.Status, userInfo.Status);

            await _userRepository.UpdateAsync(filter, updateDefinition);

            #endregion

            #region 指定对象异步修改一条数据

            //var updateUserInfo = new UserInfo
            //{
            //    UserName = userInfo.UserName,
            //    Password = MD5Helper.MDString(userInfo.Password),
            //    Status = 1,
            //    HeadPortrait = userInfo.HeadPortrait,
            //    Email = userInfo.Email,
            //    NickName = userInfo.NickName,
            //    UpdateDate = DateTime.Now,
            //};
            //await _userRepository.UpdateAsync(updateUserInfo, id);

            #endregion

            #region 数据批量修改示例

            ////1.批量修改的条件(把创建时间CreateDate为近五日的用户状态更改为0)
            //var time = DateTime.Now;
            //var list = new List<FilterDefinition<UserInfo>>();
            //list.Add(Builders<UserInfo>.Filter.Gt("CreateDate", time));//大于当前时间
            //list.Add(Builders<UserInfo>.Filter.Lt("CreateDate", time.AddDays(5)));//小于当前时间+5day
            //var filter = Builders<UserInfo>.Filter.And(list);

            ////2.要修改的字段内容
            //var dic = new Dictionary<string, string>
            //{
            //    { "Status", "0" }
            //};

            ////3.批量修改
            //await _userRepository.UpdateManayAsync(dic, filter);

            #endregion

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
