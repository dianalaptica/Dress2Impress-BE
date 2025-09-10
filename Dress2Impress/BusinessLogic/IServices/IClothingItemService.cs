using Dress2Impress.Domain.Requests;
using Dress2Impress.Domain.Responses;

namespace Dress2Impress.BusinessLogic.IServices;

public interface IClothingItemService
{
    Task<ClothingItemResponse> AddAsync(AddClothingItemRequest request, int userId);
    Task<List<ClothingItemResponse>> GetAllForUserAsync(int userId);
    Task<List<ClothingItemResponse>> GetAllByCategoryAsync(int userId, int categoryId);
    Task<List<ClothingItemResponse>> GetTopWornAsync(int userId, int limit = 4);
    Task<ClothingItemResponse?> WearNowAsync(int userId, int clothingItemId);
    Task<ClothingItemResponse?> UpdateFlagsAsync(int userId, int clothingItemId, UpdateItemFlagsRequest request);
    Task<List<ClothingItemResponse>> GetDirtyAsync(int userId);
    Task<int> WashItemsAsync(int userId, IEnumerable<int> itemIds);
}

