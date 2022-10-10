using Microsoft.EntityFrameworkCore;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace ShopLogic.Servise
{
    public class LocalDbServiceProducts
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
        public Product GetProductById(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Products.Where(x => x.Id == id).FirstOrDefault();
            }
        }
        public ProductResponse GetListProductOnPage (ApplicationContext db, PagingWithSerchingFilterProducts serchingFilter)
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
        }
    }

