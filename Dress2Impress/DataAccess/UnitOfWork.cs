using Dress2Impress.DataAccess.IRepositories;
using Dress2Impress.DataAccess.Repositories;
using Dress2Impress.Domain.DBContext;
using Dress2Impress.Domain.Models;

namespace Dress2Impress.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly Dress2ImpressContext _context;

    public IUserRepository UserRepository { get; }
    public IClothingItemRepository ClothingItemRepository { get; }
    public IOutfitRepository OutfitRepository { get; }

    public IGenericRepository<ColourRule> ColourRulesRepository { get; }
    public IGenericRepository<StyleRule> StyleRulesRepository { get; }

    public UnitOfWork(Dress2ImpressContext context)
    {
        _context = context;
        UserRepository = new UserRepository(_context);
        ClothingItemRepository = new ClothingItemRepository(_context);
        OutfitRepository = new OutfitRepository(_context);

        ColourRulesRepository = new GenericRepository<ColourRule>(_context);
        StyleRulesRepository = new GenericRepository<StyleRule>(_context);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
