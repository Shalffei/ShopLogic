using Microsoft.EntityFrameworkCore;
using ShopLogic.EntityFramework;
using ShopLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLogic.Servise
{
    public class LocalDbServiceStatistic
    {
        public TimofeyModel2 GetDateOrdersWithUserTimofeyEdition(ApplicationContext db, DateTime start, DateTime finish)
        {
            TimofeyModel2 response = new TimofeyModel2 { Users = new List<TimofeyModelUserData>() };
            var orders = db.Orders.AsNoTracking().Where(x => x.IsPayed == true && x.Created >= start && x.Created <= finish).ToList();
            var ordersPerUserId = orders.ToLookup(x => x.UserId);
            var userIdList = orders.Select(x => x.UserId).Distinct().ToList();
            var userNameById = db.Users.Where(x => userIdList.Contains(x.Id)).Select(x => new User { Id = x.Id, Name = x.Name })
                .ToDictionary(x => x.Id, x => x.Name);
            var productIdList = orders.Select(x => x.ProductId).Distinct().ToList();
            var productById = db.Products.Where(x => productIdList.Contains(x.Id))
                .Select(x => new Product { Id = x.Id, Name = x.Name, Price = x.Price }).ToDictionary(x => x.Id);
            response.PopularProductId = orders.GroupBy(x => x.ProductId).OrderByDescending(x => x.Count()).First().Key;
            foreach (var userId in userIdList)
            {
                var thisUserOrders = ordersPerUserId[userId].ToList();
                var productsPerOrder = thisUserOrders.ToLookup(x => x.ProductId);
                TimofeyModelUserData user = new TimofeyModelUserData { ProductData = new List<TimofeyModelProductData>() };
                user.UserName = userNameById[userId];
                user.UserId = userId;
                user.TotalSum = thisUserOrders.Sum(x => x.Price);
                var orderProductIdList = thisUserOrders.Select(x => x.ProductId).Distinct().ToList();
                foreach (var productId in orderProductIdList)
                {
                    var productInfo = productById[productId];
                    var thisOrderProducts = productsPerOrder[productId].ToList();
                    TimofeyModelProductData product = new TimofeyModelProductData();
                    product.ProductId = productId;
                    product.Sum = thisOrderProducts.Sum(x => x.Price);
                    product.ProductCount = thisOrderProducts.Count;
                    product.Price = productInfo.Price;
                    product.ProductName = productInfo.Name;
                    user.ProductData.Add(product);
                }
                response.Users.Add(user);
            }

            return response;
        }



        public TimofeyModel2 GetDateOrdersWithUser(ApplicationContext db, DateTime start, DateTime finish)
        {
            TimofeyModel2 timofeyModel2 = new TimofeyModel2 { Users = new List<TimofeyModelUserData>() };
            timofeyModel2.PopularProductId = db.Products.Include(prod => prod.ProductOrders.Where(order => order.Created >= start && order.Created <= finish)).OrderByDescending(x => x.BuyCount).Select(x => x.Id).FirstOrDefault();
            var usersId = db.Users.Include(user => user.UserOrders.Where(order => order.Created >= start && order.Created <= finish)).Select(user => user.Id).ToList();
            for (int i = 0; i < usersId.Count; i++)
            {
                TimofeyModelUserData user = new TimofeyModelUserData { ProductData = new List<TimofeyModelProductData>() };
                user.UserId = usersId[i];
                user.UserName = db.Users.Where(user => user.Id == usersId[i]).Select(user => user.Name).Single();
                user.TotalSum = db.Orders.Where(order => order.Created >= start && order.Created <= finish && order.UserId == usersId[i]).Sum(order => order.Price);
                var productId = db.Orders.Where(order => order.IsPayed == true && order.Created >= start && order.Created <= finish && order.UserId == usersId[i]).Select(x => x.Product.Id).ToList();
                for (int j = 0; j < productId.Count; j++)
                {
                    TimofeyModelProductData product = new TimofeyModelProductData();
                    product.ProductId = productId[j];
                    product.ProductName = db.Products.Where(prod => prod.Id == productId[j]).Select(prod => prod.Name).Single();
                    product.Price = db.Products.Where(prod => prod.Id == productId[j]).Select(prod => prod.Price).FirstOrDefault();
                    product.ProductCount = db.Orders.Where(order => order.ProductId == productId[j] && order.UserId == usersId[i] && order.Created >= start && order.Created <= finish).Count();
                    product.Sum = db.Orders.Where(order => order.ProductId == productId[j] && order.UserId == usersId[i] && order.Created >= start && order.Created <= finish).Sum(x => x.Price);
                    user.ProductData.Add(product);
                }
                timofeyModel2.Users.Add(user);
            }
            return timofeyModel2;
        }
    }
}


            //    //TimofeyModel timofeyModel = new TimofeyModel();
            //    //timofeyModel.Users = db.Users.Include(x => x.UserOrders.Where(x => x.Created >= start && x.Created <= finish)).ToList();
            //    //timofeyModel.PopularProductId = db.Products.Include(x => x.ProductOrders.Where(x => x.Created >= start && x.Created <= finish)).OrderByDescending(x => x.BuyCount).Select(x => x.Id).FirstOrDefault();
            //    //foreach (var user in timofeyModel.Users)
            //    //{
            //    //    user.AllCountOrders = user.UserOrders.Count;
            //    //    user.TotalAmount = user.UserOrders.Sum(x => x.Price);
            //    //}
            //    return timofeyModel2;
            //}
 
