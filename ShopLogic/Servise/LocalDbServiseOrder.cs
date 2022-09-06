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
        public string AddToDbOrder(ApplicationContext db, Order order, int userId)
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
        public void RemoveFromDbOrder(ApplicationContext db, Order order)
        {
            db.Remove(order);
            db.SaveChanges();
            Console.WriteLine("Remuve successfuly");
        }
        public void ChangesToDbOrder(ApplicationContext db, Order order)
        {
            db.Orders.Update(order);
            db.SaveChanges();
            Console.WriteLine("Changes was saved");
        }
        public List<Order> GetFromDbOrderList(ApplicationContext db)
        {
            List<Order> result = db.Orders.ToList();
            return result;
        }
        public void BuyOrders(ApplicationContext db, int userId, List<int> ordersId)
        {
            List<Order> orders = db.Orders.Where(x=> x.UserId == userId).ToList();
            User user = db.Users.Where(x => x.Id == userId).Single();
            decimal totalOrdersPrice = orders.Sum(x => x.Price);
            if (totalOrdersPrice <= user.MoneyBalance)
            {
                user.MoneyBalance -= totalOrdersPrice;
                db.Users.Update(user);
                foreach (var item in orders)
                {
                    item.IsPayed = true;
                }
                db.SaveChanges();
                Console.WriteLine("Orders successfully purchased");
            }
            else
            {
                Console.WriteLine("You don't have enough money");
            }
        }
        public void RemoveOrderFromTrash (ApplicationContext db, int userId, List<int> ordersId)
        {
            List<Order> orders = db.Orders.Where(x => x.UserId == userId).ToList();

            for (int i = 0; i < orders.Count; i++)
            {
                for (int j = 0; j < ordersId.Count; j++)
                {
                    if (orders[i].Id == ordersId[j] && orders[i].IsPayed != true)
                    {
                        db.Orders.Remove(orders[i]);
                        db.SaveChanges();
                    }
                    else
                        continue;
                }
            }
        }
    }
}
