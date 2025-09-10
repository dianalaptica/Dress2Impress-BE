using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.Domain.Models;

[Index("Style1Id", "Style2Id", Name = "UX_StyleRules_OrderedPair", IsUnique = true)]
public partial class StyleRule
{
    [Key]
    public int Id { get; set; }

    public int Style1Id { get; set; }

    public int Style2Id { get; set; }

    [ForeignKey("Style1Id")]
    [InverseProperty("StyleRuleStyle1s")]
    public virtual Style Style1 { get; set; } = null!;

    [ForeignKey("Style2Id")]
    [InverseProperty("StyleRuleStyle2s")]
    public virtual Style Style2 { get; set; } = null!;
}
