using Dress2Impress.BusinessLogic.IServices;
using Dress2Impress.DataAccess;
using Dress2Impress.Domain.Models;
using Dress2Impress.Domain.Requests;
using Dress2Impress.Domain.Responses;

namespace Dress2Impress.BusinessLogic.Services;

public class OutfitService : IOutfitService
{
    private readonly IUnitOfWork _uow;

    private const int CATEGORY_TOPS = 1;
    private const int CATEGORY_BOTTOMS = 2;
    private const int CATEGORY_SHOES = 3;

    public OutfitService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<OutfitResponse>> GetAllForUserAsync(int userId)
    {
        var outfits = await _uow.OutfitRepository.GetByUserIdAsync(userId);
        return outfits.Select(MapToResponse).ToList();
    }

    public async Task<WearTodayResponse> WearTodayAsync(int userId, int outfitId)
    {
        var outfit = await _uow.OutfitRepository.GetByIdForUserWithItemsAsync(userId, outfitId);
        if (outfit == null)
            throw new KeyNotFoundException("Outfit not found.");

        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        var wasUpdated = false;
        if (outfit.WearOn != today)
        {
            outfit.WearOn = today;
            outfit.NumberOfWears += 1;
            foreach (var oci in outfit.OutfitClothingItems)
            {
                if (oci.Clothing != null)
                {
                    oci.Clothing.LastWore = today;
                    oci.Clothing.NumberOfWears += 1;
                }
            }
            wasUpdated = true;
            await _uow.SaveAsync();
        }

        return new WearTodayResponse
        {
            WasUpdated = wasUpdated,
            Outfit = MapToResponse(outfit)
        };
    }

    public async Task<List<OutfitItemResponse>> GenerateForStyleAsync(int userId, int styleId)
    {
        var colourRules = await _uow.ColourRulesRepository.GetAllAsync();
        var styleRules = await _uow.StyleRulesRepository.GetAllAsync();

        bool AreColoursMatching(int? a, int? b)
        {
            if (!a.HasValue || !b.HasValue) return false;
            if (a.Value == b.Value) return true;
            return colourRules.Any(r => (r.Colour1Id == a.Value && r.Colour2Id == b.Value) || (r.Colour1Id == b.Value && r.Colour2Id == a.Value));
        }

        bool AreStylesMatching(int? a, int? b)
        {
            if (!a.HasValue || !b.HasValue) return false;
            if (a.Value == b.Value) return true;
            return styleRules.Any(r => (r.Style1Id == a.Value && r.Style2Id == b.Value) || (r.Style1Id == b.Value && r.Style2Id == a.Value));
        }

        var tops = await _uow.ClothingItemRepository.GetActiveByUserAndCategoryAsync(userId, CATEGORY_TOPS);
        var bottoms = await _uow.ClothingItemRepository.GetActiveByUserAndCategoryAsync(userId, CATEGORY_BOTTOMS);
        var shoes = await _uow.ClothingItemRepository.GetActiveByUserAndCategoryAsync(userId, CATEGORY_SHOES);

        tops = tops
            .Where(t => t.StyleId.HasValue && t.StyleId == styleId)
            .ToList();

        ShuffleInPlace(tops);
        ShuffleInPlace(bottoms);
        ShuffleInPlace(shoes);

        foreach (var top in tops)
        {
            var bottomsOk = bottoms
                .Where(b => b.StyleId.HasValue && AreStylesMatching(b.StyleId, top.StyleId) && AreColoursMatching(b.ColourId, top.ColourId))
                .ToList();

            ShuffleInPlace(bottomsOk);

            foreach (var bottom in bottomsOk)
            {
                var shoesOk = shoes
                    .Where(s => s.StyleId.HasValue && AreStylesMatching(s.StyleId, top.StyleId) && AreStylesMatching(s.StyleId, bottom.StyleId)
                         && AreColoursMatching(s.ColourId, top.ColourId) && AreColoursMatching(s.ColourId, bottom.ColourId))
                    .ToList();

                ShuffleInPlace(shoesOk);

                var shoe = shoesOk.FirstOrDefault();
                if (shoe != null)
                {
                    if (await OutfitExistsForUserAsync(userId, top.Id, bottom.Id, shoe.Id))
                        continue;

                    return new List<OutfitItemResponse>
                        {
                            new OutfitItemResponse { ImageUrl = top.ImageUrl ?? string.Empty,    Position = 1, ClothingItemId = top.Id },
                            new OutfitItemResponse { ImageUrl = bottom.ImageUrl ?? string.Empty, Position = 2, ClothingItemId = bottom.Id },
                            new OutfitItemResponse { ImageUrl = shoe.ImageUrl ?? string.Empty,   Position = 3, ClothingItemId = shoe.Id },
                        };
                }
            }
        }
        return new List<OutfitItemResponse>();
    }

    public async Task<OutfitResponse> AcceptAsync(int userId, AcceptOutfitRequest request)
    {
        var itemIds = request.Items.Select(i => i.ClothingItemId).ToList();

        var allItems = await _uow.ClothingItemRepository.GetAllAsync();
        var itemsById = allItems.Where(ci => itemIds.Contains(ci.Id) && ci.UserId == userId && !ci.WornOut && !ci.IsDirty).ToList();
        if (itemsById.Count != 3)
            throw new ArgumentException("Invalid clothing items.");

        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        var outfit = new Outfit
        {
            OutfitName = request.OutfitName,
            UserId = userId,
            NumberOfWears = 1,
            WearOn = today,
            OutfitClothingItems = request.Items
                .Select(i => new OutfitClothingItem
                {
                    ClothingId = i.ClothingItemId,
                    Position = i.Position
                })
                .ToList()
        };

        await _uow.OutfitRepository.InsertAsync(outfit);
        await _uow.SaveAsync();

        var created = await _uow.OutfitRepository.GetByIdForUserWithItemsAsync(userId, outfit.Id) ?? outfit;

        return MapToResponse(created);
    }

    private async Task<bool> OutfitExistsForUserAsync(int userId, int topId, int bottomId, int shoesId)
    {
        var existing = await _uow.OutfitRepository.GetAllForUserWithItemsAsync(userId);
        var target = new HashSet<int>([topId, bottomId, shoesId]);

        foreach (var o in existing)
        {
            if (o.OutfitClothingItems == null) continue;
            var set = o.OutfitClothingItems.Select(oci => oci.ClothingId).ToHashSet();
            if (set.Count == 3 && set.SetEquals(target)) return true;
        }
        return false;
    }

    private static void ShuffleInPlace<T>(IList<T> list)
    {
        if (list == null || list.Count <= 1) return;
        var random = new Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private static OutfitResponse MapToResponse(Outfit o)
    {
        return new OutfitResponse
        {
            Id = o.Id,
            OutfitName = o.OutfitName ?? string.Empty,
            LastWoreDate = o.WearOn.HasValue ? o.WearOn.Value.ToString("yyyy-MM-dd") : null,
            NumberOfWears = o.NumberOfWears,
            Items = o.OutfitClothingItems
                .OrderBy(oci => oci.Position ?? int.MaxValue)
                .Select(oci => new OutfitItemResponse
                {
                    ImageUrl = oci.Clothing.ImageUrl ?? string.Empty,
                    Position = oci.Position,
                    ClothingItemId = oci.ClothingId
                })
                .ToList()
        };
    }
}
