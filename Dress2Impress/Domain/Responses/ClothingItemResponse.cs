namespace Dress2Impress.Domain.Responses
{
    public class ClothingItemResponse
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int SubcategoryId { get; set; }
        public bool IsDirty { get; set; }
        public bool WornOut { get; set; }
        public int NumberOfWears { get; set; }
        public int? ColourId { get; set; }
        public int? LocationId { get; set; }
        public int? StyleId { get; set; }
        public string? ColourName { get; set; }
        public string? LocationName { get; set; }
        public string? StyleName { get; set; }
        public string SubcategoryName { get; set; } = null!;
        public string? LastWoreDate { get; set; }
    }
}
