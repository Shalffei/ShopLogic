using Microsoft.AspNetCore.Mvc;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using ShopLogic.Servise;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopLogic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
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
        [Route("AddProducts")]
        public string AddProducts([FromBody] List<Product> products)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceProducts serviseProducts = new LocalDbServiceProducts();
                serviseProducts.AddNewProduct(db, products);
                return "Ok";
            }
        }
        [HttpPost]
        [Route("OrderStatisticsByDate")]
        public IActionResult OrderStatisticsByDate([FromBody] StartFinishDate startFinishDate)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceStatistic dbServiseStatistic = new LocalDbServiceStatistic();
                var result = dbServiseStatistic.GetDateOrdersWithUserTimofeyEdition(db, startFinishDate.StartDate, startFinishDate.EndDate);
                var serialazer = JsonSerializer.Serialize(result, BaseTextJsonSerializerWriteSettings);
                return Content(serialazer, "application/json; charset=utf-8");
            }
        }
        [HttpPost]
        [Route("GetProducts")]
        public IActionResult GetProducts([FromBody] PagingWithSerchingFilterProducts serchingFilterProducts)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                LocalDbServiceProducts localDbServiseProducts = new LocalDbServiceProducts();
                var products = localDbServiseProducts.GetListProductOnPage(db, serchingFilterProducts);
                var result = JsonSerializer.Serialize(products, BaseTextJsonSerializerWriteSettings);
                return Content(result, "application/json; charset=utf-8");
            }
        }
        [HttpGet]
        [Route("GetRozetcaProducts")]
        public IActionResult GetProductsFromRozetka([FromQuery]int page)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                int notNull = 1;
                LocalDbServiceProducts localDbServiseProducts = new LocalDbServiceProducts();
                if(page == null)
                {
                    var products = localDbServiseProducts.GetListProducts(db, notNull);
                    var result = JsonSerializer.Serialize(products, BaseTextJsonSerializerWriteSettings);
                    return Content(result, "application/json; charset=utf-8");
                }
                else
                {
                    var products = localDbServiseProducts.GetListProducts(db, page);
                    var result = JsonSerializer.Serialize(products, BaseTextJsonSerializerWriteSettings);
                    return Content(result, "application/json; charset=utf-8");
                }
            }
        }
        [HttpGet]
        [Route ("GetProductById")]
        public IActionResult GetProductById([FromQuery] int id)
        {
            LocalDbServiceProducts localDbServiceProducts = new LocalDbServiceProducts();
            var product = localDbServiceProducts.GetProductById(id);
            return Content(JsonSerializer.Serialize(product, BaseTextJsonSerializerWriteSettings));
        }
    }
}
