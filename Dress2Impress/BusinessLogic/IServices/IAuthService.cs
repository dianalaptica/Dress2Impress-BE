using Dress2Impress.Domain.Requests;
using Dress2Impress.Domain.Responses;

namespace Dress2Impress.BusinessLogic.IServices;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}
