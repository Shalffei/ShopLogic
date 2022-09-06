using Microsoft.AspNetCore.Mvc;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using ShopLogic.Servise;
using System.Text;
using System.Text.Json;


namespace ShopLogic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationContext db = new ApplicationContext();
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("AddOrder")]
        public IActionResult AddOrder([FromBody] Order model, [FromQuery] int userId)
        {
            LocalDbServiseOrder addOrder = new LocalDbServiseOrder();
            string message = addOrder.AddToDbOrder(db, model, userId);
            var resultMessage = JsonSerializer.Serialize(message);
            return Ok(resultMessage);
        }
        [HttpPost]
        [Route("AddOrders")]
        public IActionResult AddOrders([FromBody] List<Order> model, [FromQuery] int userId)
        {
            LocalDbServiseOrder addOrder = new LocalDbServiseOrder();
            string message = addOrder.AddToTrashOrders(db, model, userId);
            var resultMessage = JsonSerializer.Serialize(message);
            return Ok(resultMessage);
        }
    }
}
