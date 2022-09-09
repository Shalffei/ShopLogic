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
    public class LocalDbServiseOrder
    {
        public string AddToTrashOrder(ApplicationContext db, Order order, int userId)
        {
            order.UserId = userId;
            db.Orders.Add(order);
            db.SaveChanges();
            return "Order Added";
        }
        public string AddToTrashOrders(ApplicationContext db, List<Order> order, int userId)
        {
            foreach (var item in order)
            {
                item.UserId = userId;
            }

            foreach (var item in order)
            {
                db.Orders.Add(item);
                db.SaveChanges();

            }
            return "Object Added";
        }
        public string BuyOrders(ApplicationContext db, int userId)
        {
            List<Order> orders = db.Orders.Where(x => x.UserId == userId && x.IsPayed == false).ToList();
            User user = db.Users.Where(x => x.Id == userId).Single();
            decimal totalOrdersPrice = orders.Sum(x => x.Price);
            if (totalOrdersPrice <= user.MoneyBalance)
            {
                LocalDbServiseProducts serviseProducts = new LocalDbServiseProducts();
                serviseProducts.IncrementCountProducts(db, orders);
                user.MoneyBalance -= totalOrdersPrice;
                db.Users.Update(user);
                foreach (var item in orders)
                {
                    item.IsPayed = true;
                    item.Created = DateTime.Now;
                    db.SaveChanges();
                }
                return "Orders successfully purchased";
            }
            else
            {
                return "You don't have enough money";
            }
        }
        public string RemoveOrderFromTrash (ApplicationContext db, int userId, List<int> ordersId)
        {
            List<Order> orders = db.Orders.Where(x => x.UserId == userId && x.IsPayed != true).ToList();

            for (int j = 0; j < ordersId.Count; j++)
            {
                db.Orders.Remove(orders[j]);
                db.SaveChanges();
            }
            return "Selected orders have been deleted";
        }
        public TimofeyModel GetDateOrdersWithUser (ApplicationContext db, DateTime start, DateTime finish)
        {
            
            TimofeyModel timofeyModel = new TimofeyModel();
            timofeyModel.Users = db.Users.Include(x => x.UserOrders.Where(x => x.Created >= start && x.Created <= finish)).ToList();
            timofeyModel.PopularProductId = db.Products.Include(x => x.ProductOrders.Where(x => x.Created >= start && x.Created <= finish)).OrderByDescending(x => x.BuyCount).Select(x => x.Id).FirstOrDefault();
            foreach (var user in timofeyModel.Users)
            {
                user.AllCountOrders = user.UserOrders.Count;
                user.TotalAmount = user.UserOrders.Sum(x => x.Price);
            }
            Console.ReadLine();
            return timofeyModel;
        }
    }
}
