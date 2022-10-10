namespace ShopLogic.Models
{
    public class ProductForView : Product
    {
        public List<Characteristics> CharacteristicsList { get; set; } = new List<Characteristics>();
    }
    
}
