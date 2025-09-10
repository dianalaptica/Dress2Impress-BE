using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Table("Outfit")]
public partial class Outfit
{
    [Key]
    public int Id { get; set; }

    public DateOnly? WearOn { get; set; }

    public int UserId { get; set; }

    public int NumberOfWears { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? OutfitName { get; set; }

    [InverseProperty("Outfit")]
    public virtual ICollection<OutfitClothingItem> OutfitClothingItems { get; set; } = new List<OutfitClothingItem>();

    [ForeignKey("UserId")]
    [InverseProperty("Outfits")]
    public virtual User User { get; set; } = null!;
}
