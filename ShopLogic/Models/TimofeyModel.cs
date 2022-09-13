namespace ShopLogic.Models
{
    public class TimofeyModel
    {
        public int PopularProductId { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
    public class TimofeyModel2
    {
        public int PopularProductId { get; set; }
        public List<TimofeyModelUserData> Users { get; set; }
    }

    public class TimofeyModelUserData
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<TimofeyModelProductData> ProductData { get; set; }
        public decimal TotalSum { get; set; }
    }

    public class TimofeyModelProductData
    {
        public int ProductId { get; set; }
        public int ProductCount { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
    }
}


