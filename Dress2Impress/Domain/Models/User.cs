using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Table("User")]
[Index("Email", Name = "UQ_User_Email", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(320)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<ClothingItem> ClothingItems { get; set; } = new List<ClothingItem>();

    [InverseProperty("User")]
    public virtual ICollection<Outfit> Outfits { get; set; } = new List<Outfit>();
}
