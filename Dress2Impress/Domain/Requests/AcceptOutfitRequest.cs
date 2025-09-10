namespace Dress2Impress.Domain.Requests;

public class AcceptOutfitItemRequest
{
    public int ClothingItemId { get; set; }

    public int Position { get; set; }
}

public class AcceptOutfitRequest
{
    public string OutfitName { get; set; } = null!;

    public List<AcceptOutfitItemRequest> Items { get; set; } = new();
}
