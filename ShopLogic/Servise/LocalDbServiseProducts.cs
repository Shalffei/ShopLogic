using Microsoft.EntityFrameworkCore;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using System.Reflection.Metadata.Ecma335;

namespace ShopLogic.Servise
{
    public class LocalDbServiseProducts
    {  
        public void AddNewProduct(ApplicationContext db, List<Product> products)
        {
            foreach (Product product in products)
            {
                db.Add(product);
                db.SaveChanges();
            }
        }
        public void IncrementCountProducts(ApplicationContext db, List <Order> orders)
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
        public ProductResponse GetListProductOnPage (ApplicationContext db, int page, int countProductsOnPage, string productName)
        {
            var products = db.Products
                .Where(prod => prod.Name == productName)
                .Skip((page - 1) * countProductsOnPage)
                .Take(countProductsOnPage)
                .ToList();
            var result = new ProductResponse { Products = products, CurrentPage = page, TotalProducts = db.Products.Count() };
            return result;
        }
    }
}
