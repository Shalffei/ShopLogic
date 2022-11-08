using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShopLogic.Servise
{
    public class LocalDbServiceProducts
    {  
        public void AddNewProduct(ApplicationDbContext db, List<Product> products)
        {
            foreach (Product product in products)
            {
                db.Add(product);
                db.SaveChanges();
            }
        }
        public void IncrementCountProducts(ApplicationDbContext db, List <Order> orders)
        {
            Product products = new Product();
            
            foreach (Order order in orders)
            {
                products = db.Products.Where(x => x.Id == order.ProductId).Single();
                products.BuyCount ??= 0;
                products.BuyCount++;
                db.Update(products);
                db.SaveChanges();
            }            
        }
        public Product GetProductById(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Products.Where(x => x.IdRozetka == id).FirstOrDefault();
            }
        }
        public ProductResponse GetListProductOnPage (ApplicationDbContext db, PagingWithSerchingFilterProducts serchingFilter)
        {
            IQueryable<Product> productsIQuer = db.Products;
            List<Product> products = new List<Product>();
            if (serchingFilter.ProductFilter != null)
            {
                productsIQuer.Where(x => x.ProductCategoryName == serchingFilter.ProductFilter);
            }
            if (serchingFilter.ProductName != null)
            {
                productsIQuer.Where(x => x.Name.Contains(serchingFilter.ProductName));
            }
            if (serchingFilter.YearMade != null)
            {
                productsIQuer.Where(x => x.YearMade == serchingFilter.YearMade);
            }
            if (serchingFilter.CountryMade != null)
            {
                productsIQuer.Where(x => x.CountryMade == serchingFilter.CountryMade);
            }
            products = productsIQuer.AsNoTracking().Select(prod => new Product
            {
                Id = prod.Id,
                Name = prod.Name,
                Price = prod.Price,
                CountryMade = prod.CountryMade,
                YearMade = prod.YearMade
            }).ToList();
            ProductResponse result = new ProductResponse { Products = products, CurrentPage = serchingFilter.Page, TotalProducts = db.Products.Count() };
            return result;
        }
        public JsonForPagingWithProduct GetListProducts(ApplicationDbContext db, int page)
        {
            if (page == 0)
            {
                page = 1;
            }
            var pageResult = 4f;
            var pageCount = Math.Ceiling(db.Products.Count() / pageResult);
            var products = db.Products.Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToList();
            List<ProductForView> result = new List<ProductForView>();
            foreach (var item in products)
            {
                ProductForView product = new ProductForView() { 
                    CountryMade = item.CountryMade, 
                    IdRozetka = item.IdRozetka, 
                    Name = item.Name,
                    ProductRozetkaId = item.ProductRozetkaId,
                    ProductCategoryName = item.ProductCategoryName,
                    Price = item.Price,
                    CharacteristicsList = JsonSerializer.Deserialize<List<Characteristics>>(item.Characteristics).ToList()
                };
                result.Add(product);
            }
            JsonForPagingWithProduct jsonForPagingWithProduct = new JsonForPagingWithProduct() { TotalPages = (int)pageCount, Products = result, Page = page};
            return jsonForPagingWithProduct;
        }
    }
}

