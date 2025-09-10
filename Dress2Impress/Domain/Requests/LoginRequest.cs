using System.ComponentModel.DataAnnotations;

namespace Dress2Impress.Domain.Requests;

public class LoginRequest
{
    [Required, EmailAddress, MaxLength(320)]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
