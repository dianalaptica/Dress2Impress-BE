using Dress2Impress.Domain.Models;

namespace Dress2Impress.BusinessLogic.IServices;

public interface ITokenService
{
    string CreateToken(User user);
}
