using Dress2Impress.DataAccess.IRepositories;
using Dress2Impress.Domain.DBContext;
using Dress2Impress.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.DataAccess.Repositories;

public class OutfitRepository : GenericRepository<Outfit>, IOutfitRepository
{
    public OutfitRepository(Dress2ImpressContext context) : base(context) { }

    public Task<List<Outfit>> GetByUserIdAsync(int userId)
    {
        return _context.Outfits
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .Include(o => o.OutfitClothingItems)
                .ThenInclude(oci => oci.Clothing)
            .OrderByDescending(o => o.WearOn)
            .ThenByDescending(o => o.Id)
            .ToListAsync();
    }

    public Task<Outfit?> GetByIdForUserWithItemsAsync(int userId, int outfitId)
    {
        return _context.Outfits
            .Where(o => o.Id == outfitId && o.UserId == userId)
            .Include(o => o.OutfitClothingItems)
                .ThenInclude(oci => oci.Clothing)
            .FirstOrDefaultAsync();
    }

    public Task<List<Outfit>> GetAllForUserWithItemsAsync(int userId)
    {
        return _context.Outfits
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .Include(o => o.OutfitClothingItems)
                .ThenInclude(oci => oci.Clothing)
            .ToListAsync();
    }
}
