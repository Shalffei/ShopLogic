﻿using Microsoft.EntityFrameworkCore;
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
        public TimofeyModel2 GetDateOrdersWithUser (ApplicationContext db, DateTime start, DateTime finish)
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


            //TimofeyModel timofeyModel = new TimofeyModel();
            //timofeyModel.Users = db.Users.Include(x => x.UserOrders.Where(x => x.Created >= start && x.Created <= finish)).ToList();
            //timofeyModel.PopularProductId = db.Products.Include(x => x.ProductOrders.Where(x => x.Created >= start && x.Created <= finish)).OrderByDescending(x => x.BuyCount).Select(x => x.Id).FirstOrDefault();
            //foreach (var user in timofeyModel.Users)
            //{
            //    user.AllCountOrders = user.UserOrders.Count;
            //    user.TotalAmount = user.UserOrders.Sum(x => x.Price);
            //}
            return timofeyModel2;
        }
    }
}
