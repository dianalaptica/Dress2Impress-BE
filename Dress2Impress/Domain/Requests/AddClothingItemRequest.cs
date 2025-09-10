using System.ComponentModel.DataAnnotations;

namespace Dress2Impress.Domain.Requests;

public class AddClothingItemRequest
{
    [Required]
    public string Base64Image { get; set; } = null!;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public int SubcategoryId { get; set; }

    public int? ColourId { get; set; }

    public int? LocationId { get; set; }

    public int? StyleId { get; set; }
}
