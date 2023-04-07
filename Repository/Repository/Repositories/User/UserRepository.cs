using Repository.Domain.User;
using Repository.Interface;

namespace Repository.Repositories.User
{
    public class UserRepository : MongoBaseRepository<UserInfo>, IUserRepository
    {
        public UserRepository(IMongoContext context) : base(context)
        {
        }
    }
}
