using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Table("Location")]
public partial class Location
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Location")]
    public virtual ICollection<ClothingItem> ClothingItems { get; set; } = new List<ClothingItem>();
}
