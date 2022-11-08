namespace ShopLogic.Models
{
    public class JsonForPagingWithProduct
    {
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public List<ProductForView> Products { get; set; } = new List<ProductForView>();
    }
}
