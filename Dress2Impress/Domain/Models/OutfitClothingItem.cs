using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[PrimaryKey("OutfitId", "ClothingId")]
public partial class OutfitClothingItem
{
    [Key]
    public int OutfitId { get; set; }

    [Key]
    public int ClothingId { get; set; }

    public int? Position { get; set; }

    [ForeignKey("ClothingId")]
    [InverseProperty("OutfitClothingItems")]
    public virtual ClothingItem Clothing { get; set; } = null!;

    [ForeignKey("OutfitId")]
    [InverseProperty("OutfitClothingItems")]
    public virtual Outfit Outfit { get; set; } = null!;
}
