namespace Dress2Impress.Domain.Responses;

public class OutfitItemResponse
{
    public string ImageUrl { get; set; } = string.Empty;
    public int? Position { get; set; }
    public int? ClothingItemId { get; set; }
}
