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
            using (ApplicationContext db = new ApplicationContext())
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
            using (ApplicationContext db = new ApplicationContext())
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
            using (ApplicationContext db = new ApplicationContext())
            {
                LocalDbServiceProducts localDbServiseProducts = new LocalDbServiceProducts();
                var products = localDbServiseProducts.GetListProductOnPage(db, serchingFilterProducts);
                var result = JsonSerializer.Serialize(products, BaseTextJsonSerializerWriteSettings);
                return Content(result, "application/json; charset=utf-8");
            }
        }
        //[HttpGet]
        //[Route("GetRozetcaProducts")]
        //public IActionResult GetProductsFromRozetka()
        //{
        //     using (ApplicationContext db = new ApplicationContext())
        //    {
        //        LocalDbServiceProducts localDbServiseProducts = new LocalDbServiceProducts();
        //        var products = localDbServiseProducts.GetListProducts(db);
        //        var result = JsonSerializer.Serialize(products, BaseTextJsonSerializerWriteSettings);
        //        return Content(result, "application/json; charset=utf-8");
        //    }
        //}
    }
}
