namespace Dress2Impress.Domain.Responses;

public class OutfitResponse
{
    public int Id { get; set; }
    public string OutfitName { get; set; } = string.Empty;
    public string? LastWoreDate { get; set; }
    public int NumberOfWears { get; set; }
    public List<OutfitItemResponse> Items { get; set; } = new();
}
