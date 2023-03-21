using Repository.Domain.User;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.User
{
    public interface IUserRepository : IMongoRepository<UserInfo>
    {
    }
}
