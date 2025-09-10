using Dress2Impress.DataAccess.IRepositories;
using Dress2Impress.Domain.Models;

namespace Dress2Impress.DataAccess;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IClothingItemRepository ClothingItemRepository { get; }
    IOutfitRepository OutfitRepository { get; }

    IGenericRepository<ColourRule> ColourRulesRepository { get; }
    IGenericRepository<StyleRule> StyleRulesRepository { get; }

    Task SaveAsync();
}
