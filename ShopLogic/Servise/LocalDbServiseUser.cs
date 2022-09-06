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
    public class LocalDbServiseUser
    {
        public string AddToDbUser(ApplicationContext db, User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return "User has been created";
        }
        public void AddToDbUser(ApplicationContext db, List<User> users)
        {
            foreach (var item in users)
            {
                db.Users.Add(item);
                db.SaveChanges();
            }
            Console.WriteLine("Object Added");
        }
        public string RemoveFromDbUser(ApplicationContext db, User user)
        {
            db.Remove(user);
            db.SaveChanges();
            return "User has been remuved successfuly";


        }
        public string ChangesToDbUser(ApplicationContext db, User user)
        {
            db.Users.Update(user);
            db.SaveChanges();
            return "Changes was saved";
        }
        public List<User> GetFromDbUserList(ApplicationContext db)
        {
            List<User> result = db.Users.ToList();
            return result;    

        }
        public User GetUserBoughtOrders(ApplicationContext db, User user)
        {
            var result = db.Users.Include(x => x.UserOrders).Where(x => x.Id == user.Id).Single();
            Console.WriteLine($"All orders {user.Name}:");
            foreach (var item in result.UserOrders)
            {
                int count = 1;
                Console.WriteLine($"{count}. {item.Name} \t Price: {item.Price} \t OrerID: {item.Id} ");
            }
            return result;
        }
    }
}
