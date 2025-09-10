using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Table("Colour")]
public partial class Colour
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Colour")]
    public virtual ICollection<ClothingItem> ClothingItems { get; set; } = new List<ClothingItem>();

    [InverseProperty("Colour1")]
    public virtual ICollection<ColourRule> ColourRuleColour1s { get; set; } = new List<ColourRule>();

    [InverseProperty("Colour2")]
    public virtual ICollection<ColourRule> ColourRuleColour2s { get; set; } = new List<ColourRule>();
}
