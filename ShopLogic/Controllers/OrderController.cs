using Microsoft.AspNetCore.Mvc;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using ShopLogic.Servise;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopLogic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        [Route("AddOrders")]
        public IActionResult AddOrders([FromBody] List<Order> model, [FromQuery] int userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceOrder addOrder = new LocalDbServiceOrder();
                string message = addOrder.AddToTrashOrders(db, model, userId);
                return Content(message);
            }
        }
        public IActionResult AddOrders([FromBody] Order order, [FromQuery] int userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceOrder addOrder = new LocalDbServiceOrder();
                string message = addOrder.AddToTrashOrder(db, order, userId);
                return Content(message);
            }
        }
        [HttpGet]
        [Route("BuyOrders")]
        public IActionResult BuyOrders([FromQuery] int userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceOrder addOrder = new LocalDbServiceOrder();
                string message = addOrder.BuyOrders(db, userId);
                return Content(message);
            }
        }
        [HttpGet]
        [Route("RemoveOrdersFromTrash")]
        public IActionResult RemoveOrdersFromTrash( [FromQuery] int userId, List<int> ordersId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceOrder addOrder = new LocalDbServiceOrder();
                string message = addOrder.RemoveOrderFromTrash(db, userId, ordersId);
                return Content(message);
            }
        }        
    }
}
