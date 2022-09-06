using Microsoft.AspNetCore.Mvc;
using ShopLogic.EntityFramework;
using ShopLogic.Servise;
using ShopLogic.Models;
using System.Text.Json;
using System.IO;
using System.Text;

namespace ShopLogic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext db = new ApplicationContext();
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult GetUser ([FromBody] User model)
        {
            LocalDbServiseUser addUser = new LocalDbServiseUser();
            model.MoneyBalance = 0.0m;
            string message = addUser.AddToDbUser(db, model);
            var resultMessage = JsonSerializer.Serialize(message);
            return Ok(resultMessage);
        }
        [HttpPost]
        [Route("RemoveUser")]
        public IActionResult RemoveUser([FromBody] User model)
        {
            LocalDbServiseUser addUser = new LocalDbServiseUser();
            string message = addUser.RemoveFromDbUser(db, model);
            var resultMessage = JsonSerializer.Serialize(message);
            return Ok(resultMessage);
        }
        [HttpPost]
        [Route("ChangeUser")]
        public IActionResult ChangeUser([FromBody] User model)
        {
            LocalDbServiseUser addUser = new LocalDbServiseUser();
            string message = addUser.ChangesToDbUser(db, model);
            var resultMessage = JsonSerializer.Serialize(message);
            return Ok(resultMessage);
        }
        
    }
}