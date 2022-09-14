namespace ShopLogic.Models
{
    public class ProductResponse
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int TotalProducts { get; set; }
        public int CurrentPage { get; set; }

    }
}
