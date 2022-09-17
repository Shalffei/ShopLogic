using Microsoft.EntityFrameworkCore;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using System.Linq;
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
        public ProductResponse GetListProductOnPage (ApplicationContext db, PagingWithSerchingFilterProducts serchingFilter)
        {
            List<Product> products = new List<Product>();
            if (serchingFilter.ProductFilter != null && serchingFilter.ProductName != null)
            {
                products = db.Products
                .AsNoTracking()
                .Where(prod => prod.Name.Contains(serchingFilter.ProductName) && prod.ProductCategoryName == serchingFilter.ProductFilter)
                .Select(prod => new Product { Id = prod.Id, Name = prod.Name, Price = prod.Price })
                .Skip((serchingFilter.Page - 1) * serchingFilter.CountProductsOnPage)
                .Take(serchingFilter.CountProductsOnPage)
                .ToList();
            }
            else if (serchingFilter.ProductName != null)
            {
                products = db.Products
                .AsNoTracking()
                .Where(prod => prod.Name.Contains(serchingFilter.ProductName))
                .Select(prod => new Product { Id = prod.Id, Name = prod.Name, Price = prod.Price })
                .Skip((serchingFilter.Page - 1) * serchingFilter.CountProductsOnPage)
                .Take(serchingFilter.CountProductsOnPage)
                .ToList();
                
            }
            else if (serchingFilter.ProductFilter != null)
            {
                products = db.Products
                .AsNoTracking()
                .Where(prod => prod.Name == serchingFilter.ProductFilter)
                .Select(prod => new Product { Id = prod.Id, Name = prod.Name, Price = prod.Price })
                .Skip((serchingFilter.Page - 1) * serchingFilter.CountProductsOnPage)
                .Take(serchingFilter.CountProductsOnPage)
                .ToList();
            }
            else  
            {
                 products = db.Products
                 .AsNoTracking()
                 .Select(prod => new Product { Id = prod.Id, Name = prod.Name, Price = prod.Price })
                 .Skip((serchingFilter.Page - 1) * serchingFilter.CountProductsOnPage)
                 .Take(serchingFilter.CountProductsOnPage)
                 .ToList();
            }
            var result = new ProductResponse { Products = products, CurrentPage = serchingFilter.Page, TotalProducts = db.Products.Count() };
            return result;
        }
    }
}
