using Dress2Impress.Domain.Models;

namespace Dress2Impress.DataAccess.IRepositories
{
    public interface IClothingItemRepository : IGenericRepository<ClothingItem>
    {
        Task<List<ClothingItem>> GetByUserIdAsync(int userId);
        Task<List<ClothingItem>> GetActiveByUserAndCategoryAsync(int userId, int mainCategoryId);
        Task<List<ClothingItem>> GetByUserAndCategoryAsync(int userId, int categoryId);
        Task<List<ClothingItem>> GetTopWornByUserAsync(int userId, int limit);
        Task<ClothingItem?> GetByIdForUserAsync(int userId, int itemId);
        Task<List<ClothingItem>> GetDirtyForUserAsync(int userId);
        Task<List<ClothingItem>> GetByIdsForUserAsync(int userId, IEnumerable<int> ids);
    }
}
