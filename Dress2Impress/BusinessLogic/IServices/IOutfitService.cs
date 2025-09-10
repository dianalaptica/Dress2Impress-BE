using Dress2Impress.Domain.Requests;
using Dress2Impress.Domain.Responses;

namespace Dress2Impress.BusinessLogic.IServices;

public interface IOutfitService
{
    Task<List<OutfitResponse>> GetAllForUserAsync(int userId);
    Task<WearTodayResponse> WearTodayAsync(int userId, int outfitId);
    Task<List<OutfitItemResponse>> GenerateForStyleAsync(int userId, int styleId);
    Task<OutfitResponse> AcceptAsync(int userId, AcceptOutfitRequest request);
}
