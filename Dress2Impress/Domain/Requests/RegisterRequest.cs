using System.ComponentModel.DataAnnotations;

namespace Dress2Impress.Domain.Requests;

public class RegisterRequest
{
    [Required, MaxLength(255)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(255)]
    public string LastName { get; set; } = null!;

    [Required, EmailAddress, MaxLength(320)]
    public string Email { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;
}
