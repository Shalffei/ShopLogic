namespace ShopLogic.Models
{
    public class PagingWithSerchingFilterProducts
    {
        public int Page { get; set; }
        public int CountProductsOnPage { get; set; }
        public string? ProductName { get; set; }
        public string? ProductFilter { get; set; }
        public int? YearMade { get; set; }
        public string? CountryMade { get; set; }
    }
}
