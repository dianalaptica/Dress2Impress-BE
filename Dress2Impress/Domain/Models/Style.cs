using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Table("Style")]
public partial class Style
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Style")]
    public virtual ICollection<ClothingItem> ClothingItems { get; set; } = new List<ClothingItem>();

    [InverseProperty("Style1")]
    public virtual ICollection<StyleRule> StyleRuleStyle1s { get; set; } = new List<StyleRule>();

    [InverseProperty("Style2")]
    public virtual ICollection<StyleRule> StyleRuleStyle2s { get; set; } = new List<StyleRule>();
}
