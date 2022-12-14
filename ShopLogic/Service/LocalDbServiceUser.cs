using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using ShopLogic.EntityFramework;
using ShopLogic.Models;

namespace ShopLogic.Servise
{
    public class LocalDbServiceUser
    {
        public string AddToDbUser(ApplicationDbContext db, User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return "User has been created";
        }
        public string RemoveFromDbUser(ApplicationDbContext db, User user)
        {
            db.Remove(user);
            db.SaveChanges();
            return "User has been remuved successfuly";
        }
        public string ChangesToDbUser(ApplicationDbContext db, User user)
        {
            db.Users.Update(user);
            db.SaveChanges();
            return "Changes has saved";
        }
        public List<Order> GetUserBoughtOrders(ApplicationDbContext db, int userId)
        {
            var result = db.Orders.Where(x => x.Id == userId).ToList();
            return result;
        }
        public User GetUser(ApplicationDbContext db, int userId)
        {
            return db.Users.Where(x => x.Id == userId).Single();
        }
    }
}
