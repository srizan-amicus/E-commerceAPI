namespace EcommerceAPI.DTOs
{
    public class ProductQueryDto
    {
        public string? Search { get; set; }

        public List<int>? CategoryIds { get; set; }

        public List<int>? BrandIds { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public decimal? MinRating { get; set; }

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}