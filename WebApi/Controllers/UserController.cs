using Application.User;
using Application.User.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Repository.Domain.User;

namespace WebApi.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="userServices">userServices</param>
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetAllUserInfos()
        {
            var userInfos = await _userServices.GetAllUserInfos();
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
            var userInfo = await _userServices.GetUserInfoById(id);
            return Ok(userInfo);
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> AddUserInfo([FromBody] UserInfoReq userInfo)
        {
            var result = await _userServices.AddUserInfo(userInfo);
            return Ok(result);
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userInfo">userInfo</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfo>> UpdateUserInfo(string id, [FromBody] UserInfoReq userInfo)
        {
            var updateUserInfo = await _userServices.UpdateUserInfo(id, userInfo);
            return Ok(updateUserInfo);
        }

        /// <summary>
        /// 用户信息删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            var result = await _userServices.Delete(id);
            return Ok(result);
        }
    }
}
