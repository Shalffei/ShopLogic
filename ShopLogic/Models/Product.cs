namespace ShopLogic.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? BuyCount { get; set; }
        public List<Order>? ProductOrders { get; set; }
    }
}
