using Microsoft.AspNetCore.Mvc;
using ShopLogic.Servise;
using ShopLogic.Models;
using System.Text.Json;

namespace ShopLogic.ViewControllers
{
    public class ProductsController : Controller
    {
        [HttpGet]
        [Route("GetProduct")]
        public IActionResult GetProducts([FromQuery] int id)
        {
            LocalDbServiceProducts serviceProduct = new LocalDbServiceProducts();
            var product = serviceProduct.GetProductById(id);
            var characteristics = JsonSerializer.Deserialize<List<Characteristics>>(product.Characteristics);
            ProductForView productWithCharacteristics = new ProductForView() { Name = product.Name, IdRozetka = product.IdRozetka, Price = product.Price, ProductCategoryName = product.ProductCategoryName, CharacteristicsList = characteristics};
            return View(productWithCharacteristics);
        }
    }
}
