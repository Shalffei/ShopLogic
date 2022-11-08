namespace ShopLogic.Models
{
    public class ProductForView
    {
        public int IdRozetka { get; set; }
        public string ProductRozetkaId { get; set; }
        public string? CountryMade { get; set; }
        public string ProductCategoryName { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Characteristics> CharacteristicsList { get; set; } = new List<Characteristics>();
    }
    
}
