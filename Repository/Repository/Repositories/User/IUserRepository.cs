using Repository.Domain.User;
using Repository.Interface;

namespace Repository.Repositories.User
{
    public interface IUserRepository : IMongoRepository<UserInfo>
    {
    }
}
