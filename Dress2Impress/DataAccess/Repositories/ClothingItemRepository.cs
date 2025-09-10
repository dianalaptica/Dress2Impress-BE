using Dress2Impress.DataAccess.IRepositories;
using Dress2Impress.Domain.DBContext;
using Dress2Impress.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.DataAccess.Repositories;

public class ClothingItemRepository : GenericRepository<ClothingItem>, IClothingItemRepository
{
    public ClothingItemRepository(Dress2ImpressContext context) : base(context) { }

    public Task<List<ClothingItem>> GetByUserIdAsync(int userId)
    {
        return _context.ClothingItems
            .AsNoTracking()
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Subcategory)
            .Include(ci => ci.Colour)
            .Include(ci => ci.Location)
            .Include(ci => ci.Style)
            .OrderBy(ci => ci.WornOut)
            .ThenByDescending(ci => ci.Id)
            .ToListAsync();
    }

    public Task<List<ClothingItem>> GetActiveByUserAndCategoryAsync(int userId, int mainCategoryId)
    {
        return _context.ClothingItems
            .AsNoTracking()
            .Where(ci => ci.UserId == userId
                         && !ci.WornOut
                         && !ci.IsDirty
                         && ci.Subcategory.CategoryId == mainCategoryId)
            .Include(ci => ci.Subcategory)
            .ToListAsync();
    }

    public async Task<List<ClothingItem>> GetByUserAndCategoryAsync(int userId, int categoryId)
    {
        return await _context.ClothingItems
            .AsNoTracking()
            .Where(ci => ci.UserId == userId && ci.Subcategory.CategoryId == categoryId)
            .Include(ci => ci.Subcategory)
            .Include(ci => ci.Colour)
            .Include(ci => ci.Location)
            .Include(ci => ci.Style)
            .OrderBy(ci => ci.WornOut)
            .ToListAsync();
    }

    public async Task<List<ClothingItem>> GetTopWornByUserAsync(int userId, int limit)
    {
        return await _context.ClothingItems
            .AsNoTracking()
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Subcategory)
            .Include(ci => ci.Colour)
            .Include(ci => ci.Location)
            .Include(ci => ci.Style)
            .OrderByDescending(ci => ci.NumberOfWears)
            .ThenByDescending(ci => ci.LastWore)
            .ThenBy(ci => ci.WornOut)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<ClothingItem?> GetByIdForUserAsync(int userId, int itemId)
    {
        return await _context.ClothingItems
            .Include(ci => ci.Subcategory)
            .Include(ci => ci.Colour)
            .Include(ci => ci.Location)
            .Include(ci => ci.Style)
            .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.UserId == userId);
    }

    public async Task<List<ClothingItem>> GetDirtyForUserAsync(int userId)
    {
        return await _context.ClothingItems
            .Include(ci => ci.Subcategory)
            .Include(ci => ci.Colour)
            .Include(ci => ci.Location)
            .Include(ci => ci.Style)
            .Where(ci => ci.UserId == userId && ci.IsDirty)
            .OrderBy(ci => ci.Id)
            .ToListAsync();
    }

    public async Task<List<ClothingItem>> GetByIdsForUserAsync(int userId, IEnumerable<int> ids)
    {
        var idsList = ids.Distinct().ToList();
        if (idsList.Count == 0) return new List<ClothingItem>();

        return await _context.ClothingItems
            .Where(ci => ci.UserId == userId && idsList.Contains(ci.Id))
            .ToListAsync();
    }
}
