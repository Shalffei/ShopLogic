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

        // базовые настройки сериализатора textJson, пишу сюда ибо настройки много места занимают
        public static readonly JsonSerializerOptions BaseTextJsonSerializerWriteSettings = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static readonly JsonSerializerOptions BaseTextJsonSerializerReadSettings = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        [HttpPost]
        [Route("AddOrders")]
        public IActionResult AddOrders([FromBody] List<Order> model, [FromQuery] int userId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                LocalDbServiseOrder addOrder = new LocalDbServiseOrder();
                string message = addOrder.AddToTrashOrders(db, model, userId);
                return Content(message);
            }
        }
        public IActionResult AddOrders([FromBody] Order order, [FromQuery] int userId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                LocalDbServiseOrder addOrder = new LocalDbServiseOrder();
                string message = addOrder.AddToTrashOrder(db, order, userId);
                return Content(message);
            }
        }
        [HttpGet]
        [Route("BuyOrders")]
        public IActionResult BuyOrders([FromQuery] int userId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                LocalDbServiseOrder addOrder = new LocalDbServiseOrder();
                string message = addOrder.BuyOrders(db, userId);
                return Content(message);
            }
        }
        [HttpGet]
        [Route("RemoveOrdersFromTrash")]
        public IActionResult RemoveOrdersFromTrash( [FromQuery] int userId, List<int> ordersId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                LocalDbServiseOrder addOrder = new LocalDbServiseOrder();
                string message = addOrder.RemoveOrderFromTrash(db, userId, ordersId);
                return Content(message);
            }
        }        
    }
}
