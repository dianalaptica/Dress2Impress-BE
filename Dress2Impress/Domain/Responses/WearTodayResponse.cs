namespace Dress2Impress.Domain.Responses;

public class WearTodayResponse
{
    public bool WasUpdated { get; set; }
    public OutfitResponse Outfit { get; set; } = new OutfitResponse();
}
