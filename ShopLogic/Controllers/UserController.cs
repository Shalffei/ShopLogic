using Microsoft.AspNetCore.Mvc;
using ShopLogic.EntityFramework;
using ShopLogic.Servise;
using ShopLogic.Models;
using System.Text.Json;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace ShopLogic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
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
        [Route("AddUser")]
        public IActionResult GetUser ([FromBody] User model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceUser addUser = new LocalDbServiceUser();
                model.MoneyBalance = 3000.0m;
                string message = addUser.AddToDbUser(db, model);
                return Ok(message);
            }
        }
        [HttpPost]
        [Route("RemoveUser")]
        public IActionResult RemoveUser([FromBody] User model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceUser addUser = new LocalDbServiceUser();
                string message = addUser.RemoveFromDbUser(db, model);
                return Ok(message);
            }
        }
        [HttpPost]
        [Route("ChangeUser")]
        public IActionResult ChangeUser([FromBody] User model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceUser addUser = new LocalDbServiceUser();
                string message = addUser.ChangesToDbUser(db, model);
                return Ok(message);
            }
        }
        [HttpGet]
        [Route("GetBouthOrders")]
        public IActionResult GetBouthOrders([FromQuery] int userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceUser addUser = new LocalDbServiceUser();
                var result = addUser.GetUserBoughtOrders(db, userId);
                var serialize = JsonSerializer.Serialize(result,BaseTextJsonSerializerWriteSettings);
                return Content(serialize, "application/json; charset=utf-8");
            }
        }

    }
}