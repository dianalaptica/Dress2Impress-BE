using Dress2Impress.DataAccess.IRepositories;
using Dress2Impress.Domain.DBContext;
using Dress2Impress.Domain.Models;

namespace Dress2Impress.DataAccess.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(Dress2ImpressContext context) : base(context) { }
}
