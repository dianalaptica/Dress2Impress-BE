using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Table("ClothingSubcategory")]
public partial class ClothingSubcategory
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("ClothingSubcategories")]
    public virtual ClothingCategory Category { get; set; } = null!;

    [InverseProperty("Subcategory")]
    public virtual ICollection<ClothingItem> ClothingItems { get; set; } = new List<ClothingItem>();
}
