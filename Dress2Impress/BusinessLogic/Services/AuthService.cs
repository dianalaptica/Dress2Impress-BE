using Dress2Impress.BusinessLogic.IServices;
using Dress2Impress.DataAccess;
using Dress2Impress.Domain.Models;
using Dress2Impress.Domain.Requests;
using Dress2Impress.Domain.Responses;

namespace Dress2Impress.BusinessLogic.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly ITokenService _tokenService;
    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService) : base(unitOfWork) 
    {
        _tokenService = tokenService;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        if (await UserExists(request.Email) == false)
            return null;
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == request.Email);
        if (!IsPasswordValid(request.Password, user.PasswordHash, user.PasswordSalt))
            return null;
        string token = _tokenService.CreateToken(user);
        AuthResponse response = new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = token
        };
        return response;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        if (await UserExists(request.Email))
            return null;
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        User user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        await _unitOfWork.UserRepository.InsertAsync(user);
        await _unitOfWork.SaveAsync();
        string token = _tokenService.CreateToken(user);
        AuthResponse response = new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = token
        };
        return response;
    }

    private async Task<bool> UserExists(string email)
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == email);
        var exists = user != null;
        return exists;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private bool IsPasswordValid(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}
