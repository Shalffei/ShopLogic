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
            IQueryable<Product> productsIQuer = db.Products;
            List <Product> products = new List<Product>();
            if (serchingFilter.ProductFilter != null && serchingFilter.ProductName != null)
            {
                productsIQuer = productsIQuer
               .AsNoTracking().Where(prod => prod.Name.Contains(serchingFilter.ProductName)
                    && prod.ProductCategoryName == serchingFilter.ProductFilter);
                if (serchingFilter.YearMade != null && serchingFilter.CountryMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.YearMade == serchingFilter.YearMade
                    && prod.CountryMade == serchingFilter.CountryMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        CountryMade = prod.CountryMade,
                        YearMade = prod.YearMade
                    });
                }
                else if (serchingFilter.YearMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.YearMade == serchingFilter.YearMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        YearMade = prod.YearMade
                    });
                }
                else if (serchingFilter.CountryMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.CountryMade == serchingFilter.CountryMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        CountryMade = prod.CountryMade
                    });
                }
                else
                {
                    productsIQuer = productsIQuer.Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price
                    });
                }
                productsIQuer = productsIQuer
                .Skip((serchingFilter.Page - 1) * serchingFilter.CountProductsOnPage)
                .Take(serchingFilter.CountProductsOnPage);
                products = productsIQuer.ToList();
                ProductResponse result = new ProductResponse { Products = products, CurrentPage = serchingFilter.Page, TotalProducts = db.Products.Count() };
                return result;
            }
            else if (serchingFilter.ProductName != null)
            {
                productsIQuer = productsIQuer.AsNoTracking()
                    .Where(prod => prod.Name.Contains(serchingFilter.ProductName));
                if (serchingFilter.YearMade != null && serchingFilter.CountryMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.YearMade == serchingFilter.YearMade
                    && prod.CountryMade == serchingFilter.CountryMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        CountryMade = prod.CountryMade,
                        YearMade = prod.YearMade
                    });
                }
                else if (serchingFilter.YearMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.YearMade == serchingFilter.YearMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        YearMade = prod.YearMade
                    });
                }
                else if (serchingFilter.CountryMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.CountryMade == serchingFilter.CountryMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        CountryMade = prod.CountryMade
                    });
                }
                else
                {
                    productsIQuer = productsIQuer.Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                    });
                }
                productsIQuer = productsIQuer
                .Skip((serchingFilter.Page - 1) * serchingFilter.CountProductsOnPage)
                .Take(serchingFilter.CountProductsOnPage);
                products = productsIQuer.ToList();
                ProductResponse result = new ProductResponse { Products = products, CurrentPage = serchingFilter.Page, TotalProducts = db.Products.Count() };
                return result;
            }
            else if (serchingFilter.ProductFilter != null)
            {
                productsIQuer = productsIQuer.AsNoTracking()
                .Where(prod => prod.Name == serchingFilter.ProductFilter);
                if (serchingFilter.YearMade != null && serchingFilter.CountryMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.YearMade == serchingFilter.YearMade
                    && prod.CountryMade == serchingFilter.CountryMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Price = prod.Price,
                        CountryMade = prod.CountryMade,
                        YearMade = prod.YearMade
                    });
                }
                else if (serchingFilter.YearMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.YearMade == serchingFilter.YearMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Price = prod.Price,
                        YearMade = prod.YearMade
                    });
                }
                else if (serchingFilter.CountryMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.CountryMade == serchingFilter.CountryMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Price = prod.Price,
                        CountryMade = prod.CountryMade
                    });
                }
                else
                {
                    productsIQuer = productsIQuer.Select(prod => new Product
                    {
                        Id = prod.Id,
                        Price = prod.Price
                    });
                }
                productsIQuer = productsIQuer.Skip((serchingFilter.Page - 1) * serchingFilter.CountProductsOnPage)
                .Take(serchingFilter.CountProductsOnPage);
                products = productsIQuer.ToList();
                ProductResponse result = new ProductResponse { Products = products, CurrentPage = serchingFilter.Page, TotalProducts = db.Products.Count() };
                return result;
            }
            else
            {
                if (serchingFilter.YearMade != null && serchingFilter.CountryMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.YearMade == serchingFilter.YearMade
                    && prod.CountryMade == serchingFilter.CountryMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        CountryMade = prod.CountryMade,
                        YearMade = prod.YearMade
                    });
                }
                else if (serchingFilter.YearMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.YearMade == serchingFilter.YearMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        YearMade = prod.YearMade
                    });
                }
                else if (serchingFilter.CountryMade != null)
                {
                    productsIQuer = productsIQuer.Where(prod => prod.CountryMade == serchingFilter.CountryMade)
                    .Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        CountryMade = prod.CountryMade
                    });
                }
                else
                {
                    productsIQuer = productsIQuer.Select(prod => new Product
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price
                    });
                }
                productsIQuer = productsIQuer.AsNoTracking().Skip((serchingFilter.Page - 1) * serchingFilter.CountProductsOnPage)
                .Take(serchingFilter.CountProductsOnPage);
                products = productsIQuer.ToList();
                ProductResponse result = new ProductResponse { Products = products, CurrentPage = serchingFilter.Page, TotalProducts = db.Products.Count() };
                return result;
            }
        }
    }
}
