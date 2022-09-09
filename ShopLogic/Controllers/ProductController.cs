using Microsoft.AspNetCore.Mvc;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using ShopLogic.Servise;

namespace ShopLogic.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        [Route("AddProducts")]
        public string AddProducts([FromBody] List<Product> products)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                LocalDbServiseProducts serviseProducts = new LocalDbServiseProducts();
                serviseProducts.AddNewProduct(db, products);
                return "Ok";
            }
        }
        [HttpPost]
        [Route("OrderStatisticsByDate")]
        public string OrderStatisticsByDate([FromBody] StartFinishDate startFinishDate)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                LocalDbServiseOrder serviseOrders = new LocalDbServiseOrder();
                serviseOrders.GetDateOrdersWithUser(db, startFinishDate.StartDate, startFinishDate.EndDate);
                return "Ok";
            }
        }
    }
}
