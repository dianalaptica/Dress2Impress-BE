namespace Dress2Impress.Domain.Responses;

public class AuthResponse
{
    public int Id { get; set; }                   
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!; 
}
