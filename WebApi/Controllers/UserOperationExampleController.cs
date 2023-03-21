using Application.User.ViewModel;
using Application.User;
using Microsoft.AspNetCore.Mvc;
using Repository.Domain.User;

namespace WebApi.Controllers
{
    /// <summary>
    /// MongoDB用户管理操作示例
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserOperationExampleController : ControllerBase
    {
        private readonly IUserOperationExampleServices _userOperationExampleServices;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="userOperationExampleServices">userOperationExampleServices</param>
        public UserOperationExampleController(IUserOperationExampleServices userOperationExampleServices)
        {
            _userOperationExampleServices = userOperationExampleServices;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetAllUserInfos()
        {
            var userInfos = await _userOperationExampleServices.GetAllUserInfos();
            return Ok(userInfos);
        }

        /// <summary>
        /// 通过用户ID获取对应用户信息
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfoById(string id)
        {
            var userInfo = await _userOperationExampleServices.GetUserInfoById(id);
            return Ok(userInfo);
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserInfo>> AddUserInfo([FromBody] UserInfoViewModel userInfo)
        {
            var addUserInfo = await _userOperationExampleServices.AddUserInfo(userInfo);
            return Ok(addUserInfo);
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfo>> UpdateUserInfo(string id, [FromBody] UserInfoViewModel userInfo)
        {
            var updateUserInfo = await _userOperationExampleServices.UpdateUserInfo(id, userInfo);
            return Ok(updateUserInfo);
        }

        /// <summary>
        /// 用户信息删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var deleteUser = await _userOperationExampleServices.Delete(id);
            return Ok(deleteUser);
        }
    }
}
