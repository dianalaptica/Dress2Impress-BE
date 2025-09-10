using System.Text.RegularExpressions;
using Dress2Impress.BusinessLogic.IServices;
using Dress2Impress.DataAccess;
using Dress2Impress.Domain.Models;
using Dress2Impress.Domain.Requests;
using Dress2Impress.Domain.Responses;

namespace Dress2Impress.BusinessLogic.Services
{
    public class ClothingItemService : IClothingItemService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _env;

        public ClothingItemService(IUnitOfWork uow, IWebHostEnvironment env)
        {
            _uow = uow;
            _env = env;
        }

        public async Task<List<ClothingItemResponse>> GetAllForUserAsync(int userId)
        {
            var entities = await _uow.ClothingItemRepository.GetByUserIdAsync(userId);

            var list = entities.Select(ci => new ClothingItemResponse
            {
                Id = ci.Id,
                ImageUrl = ci.ImageUrl ?? string.Empty,
                Price = ci.Price,
                SubcategoryId = ci.SubcategoryId,
                IsDirty = ci.IsDirty,
                WornOut = ci.WornOut,
                NumberOfWears = ci.NumberOfWears,
                ColourId = ci.ColourId,
                LocationId = ci.LocationId,
                StyleId = ci.StyleId,
                ColourName = ci.Colour?.Name,
                LocationName = ci.Location?.Name,
                StyleName = ci.Style?.Name,
                SubcategoryName = ci.Subcategory.Name,
                LastWoreDate = ci.LastWore.HasValue ? ci.LastWore.Value.ToString("yyyy-MM-dd") : null,
            }).ToList();

            return list;
        }

        public async Task<ClothingItemResponse> AddAsync(AddClothingItemRequest request, int userId)
        {
            var (bytes, ext) = DecodeBase64Image(request.Base64Image);
            var guid = Guid.NewGuid().ToString("N");
            var fileName = $"{guid}{ext}";
            var uploadsFolder = Path.Combine(GetProjectRoot(), "uploads");

            Directory.CreateDirectory(uploadsFolder); // idempotent
            var physicalPath = Path.Combine(uploadsFolder, fileName);
            await File.WriteAllBytesAsync(physicalPath, bytes);

            var imageUrl = $"/uploads/{fileName}";

            var item = new ClothingItem
            {
                ImageUrl = imageUrl,
                IsDirty = false,
                Price = request.Price,
                NumberOfWears = 0,
                LastWore = null,
                WornOut = false,
                UserId = userId,
                SubcategoryId = request.SubcategoryId,
                ColourId = request.ColourId,
                LocationId = request.LocationId,
                StyleId = request.StyleId
            };

            await _uow.ClothingItemRepository.InsertAsync(item);
            await _uow.SaveAsync();

            return new ClothingItemResponse
            {
                Id = item.Id,
                ImageUrl = item.ImageUrl!,
                Price = item.Price,
                SubcategoryId = item.SubcategoryId,
                IsDirty = item.IsDirty,
                WornOut = item.WornOut,
                NumberOfWears = item.NumberOfWears,
                ColourId = item.ColourId,
                LocationId = item.LocationId,
                StyleId = item.StyleId
            };
        }

        public async Task<List<ClothingItemResponse>> GetAllByCategoryAsync(int userId, int categoryId)
        {
            var entities = await _uow.ClothingItemRepository.GetByUserAndCategoryAsync(userId, categoryId);

            var list = entities.Select(ci => new ClothingItemResponse
            {
                Id = ci.Id,
                ImageUrl = ci.ImageUrl ?? string.Empty,
                Price = ci.Price,
                SubcategoryId = ci.SubcategoryId,
                SubcategoryName = ci.Subcategory?.Name ?? string.Empty,
                IsDirty = ci.IsDirty,
                WornOut = ci.WornOut,
                NumberOfWears = ci.NumberOfWears,
                ColourId = ci.ColourId,
                LocationId = ci.LocationId,
                StyleId = ci.StyleId,
                ColourName = ci.Colour?.Name,
                LocationName = ci.Location?.Name,
                StyleName = ci.Style?.Name,
                LastWoreDate = ci.LastWore.HasValue ? ci.LastWore.Value.ToString("yyyy-MM-dd") : null,
            }).ToList();

            return list;
        }

        public async Task<List<ClothingItemResponse>> GetTopWornAsync(int userId, int limit = 4)
        {
            if (limit <= 0) limit = 4;

            var entities = await _uow.ClothingItemRepository.GetTopWornByUserAsync(userId, limit);

            return entities.Select(ci => new ClothingItemResponse
            {
                Id = ci.Id,
                ImageUrl = ci.ImageUrl ?? string.Empty,
                Price = ci.Price,
                SubcategoryId = ci.SubcategoryId,
                SubcategoryName = ci.Subcategory?.Name ?? string.Empty,
                IsDirty = ci.IsDirty,
                WornOut = ci.WornOut,
                NumberOfWears = ci.NumberOfWears,
                ColourId = ci.ColourId,
                LocationId = ci.LocationId,
                StyleId = ci.StyleId,
                ColourName = ci.Colour?.Name,
                LocationName = ci.Location?.Name,
                StyleName = ci.Style?.Name
            }).ToList();
        }

        public async Task<ClothingItemResponse?> WearNowAsync(int userId, int clothingItemId)
        {
            var item = await _uow.ClothingItemRepository.GetByIdForUserAsync(userId, clothingItemId);
            if (item == null) return null;

            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            if (item.LastWore != today)
            {
                item.NumberOfWears += 1;
                item.LastWore = today;
                await _uow.SaveAsync();
            }

            return Map(item);
        }

        public async Task<ClothingItemResponse?> UpdateFlagsAsync(int userId, int clothingItemId, UpdateItemFlagsRequest request)
        {
            var item = await _uow.ClothingItemRepository.GetByIdForUserAsync(userId, clothingItemId);
            if (item == null) return null;

            if (request.IsDirty.HasValue)
                item.IsDirty = request.IsDirty.Value;

            if (request.WornOut.HasValue)
                item.WornOut = request.WornOut.Value;

            await _uow.SaveAsync();
            return Map(item);
        }

        public async Task<List<ClothingItemResponse>> GetDirtyAsync(int userId)
        {
            var items = await _uow.ClothingItemRepository.GetDirtyForUserAsync(userId);
            return items.Select(Map).ToList();
        }

        public async Task<int> WashItemsAsync(int userId, IEnumerable<int> itemIds)
        {
            var items = await _uow.ClothingItemRepository.GetByIdsForUserAsync(userId, itemIds);
            if (items.Count == 0) return 0;

            foreach (var it in items)
            {
                it.IsDirty = false;
            }

            await _uow.SaveAsync();
            return items.Count;
        }

        private string GetProjectRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !dir.GetFiles("*.csproj").Any())
            {
                dir = dir.Parent;
            }

            if (dir == null)
            {
                throw new Exception("Project root not found.");
            }

            return dir.FullName;
        }

        private static (byte[] bytes, string ext) DecodeBase64Image(string base64OrDataUri)
        {
            string ext = ".png";
            string base64 = base64OrDataUri;

            var match = Regex.Match(base64OrDataUri, @"^data:(?<mime>[\w/\-\.]+);base64,(?<data>.+)$");
            if (match.Success)
            {
                var mime = match.Groups["mime"].Value.ToLowerInvariant();
                base64 = match.Groups["data"].Value;
            }

            var bytes = Convert.FromBase64String(base64);
            return (bytes, ext);
        }

        private static ClothingItemResponse Map(ClothingItem ci) => new ClothingItemResponse
        {
            Id = ci.Id,
            ImageUrl = ci.ImageUrl ?? string.Empty,
            Price = ci.Price,
            SubcategoryId = ci.SubcategoryId,
            SubcategoryName = ci.Subcategory?.Name ?? string.Empty,
            IsDirty = ci.IsDirty,
            WornOut = ci.WornOut,
            NumberOfWears = ci.NumberOfWears,
            ColourId = ci.ColourId,
            LocationId = ci.LocationId,
            StyleId = ci.StyleId,
            ColourName = ci.Colour?.Name,
            LocationName = ci.Location?.Name,
            StyleName = ci.Style?.Name
        };
    }
}
