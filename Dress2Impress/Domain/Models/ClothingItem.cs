using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Table("ClothingItem")]
public partial class ClothingItem
{
    [Key]
    public int Id { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    public bool IsDirty { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public int NumberOfWears { get; set; }

    public DateOnly? LastWore { get; set; }

    public bool WornOut { get; set; }

    public int UserId { get; set; }

    public int SubcategoryId { get; set; }

    public int? ColourId { get; set; }

    public int? LocationId { get; set; }

    public int? StyleId { get; set; }

    [ForeignKey("ColourId")]
    [InverseProperty("ClothingItems")]
    public virtual Colour? Colour { get; set; }

    [ForeignKey("LocationId")]
    [InverseProperty("ClothingItems")]
    public virtual Location? Location { get; set; }

    [InverseProperty("Clothing")]
    public virtual ICollection<OutfitClothingItem> OutfitClothingItems { get; set; } = new List<OutfitClothingItem>();

    [ForeignKey("StyleId")]
    [InverseProperty("ClothingItems")]
    public virtual Style? Style { get; set; }

    [ForeignKey("SubcategoryId")]
    [InverseProperty("ClothingItems")]
    public virtual ClothingSubcategory Subcategory { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ClothingItems")]
    public virtual User User { get; set; } = null!;
}
