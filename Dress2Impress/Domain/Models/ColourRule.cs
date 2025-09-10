using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Index("Colour1Id", "Colour2Id", Name = "UX_ColourRules_OrderedPair", IsUnique = true)]
public partial class ColourRule
{
    [Key]
    public int Id { get; set; }

    public int Colour1Id { get; set; }

    public int Colour2Id { get; set; }

    [ForeignKey("Colour1Id")]
    [InverseProperty("ColourRuleColour1s")]
    public virtual Colour Colour1 { get; set; } = null!;

    [ForeignKey("Colour2Id")]
    [InverseProperty("ColourRuleColour2s")]
    public virtual Colour Colour2 { get; set; } = null!;
}
