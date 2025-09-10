using Dress2Impress.Domain.Models;

namespace Dress2Impress.DataAccess.IRepositories;

public interface IOutfitRepository : IGenericRepository<Outfit>
{
    Task<List<Outfit>> GetByUserIdAsync(int userId);
    Task<Outfit?> GetByIdForUserWithItemsAsync(int userId, int outfitId);
    Task<List<Outfit>> GetAllForUserWithItemsAsync(int userId);
}
