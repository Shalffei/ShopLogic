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
    public class LocalDbServiceOrder
    {
        public string AddToTrashOrder(ApplicationDbContext db, Order order, int userId)
        {
            order.UserId = userId;
            db.Orders.Add(order);
            db.SaveChanges();
            return "Order Added";
        }
        public string AddToTrashOrders(ApplicationDbContext db, List<Order> order, int userId)
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
        public string BuyOrders(ApplicationDbContext db, int userId)
        {
            List<Order> orders = db.Orders.Where(x => x.UserId == userId && x.IsPayed == false).ToList();
            User user = db.Users.Where(x => x.Id == userId).Single();
            decimal totalOrdersPrice = orders.Sum(x => x.Price);
            if (totalOrdersPrice <= user.MoneyBalance)
            {
                LocalDbServiceProducts serviseProducts = new LocalDbServiceProducts();
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
        public string RemoveOrderFromTrash(ApplicationDbContext db, int userId, List<int> ordersId)
        {
            List<Order> orders = db.Orders.Where(x => x.UserId == userId && x.IsPayed != true).ToList();

            for (int j = 0; j < ordersId.Count; j++)
            {
                db.Orders.Remove(orders[j]);
                db.SaveChanges();
            }
            return "Selected orders have been deleted";
        }

    }
}
        
    

