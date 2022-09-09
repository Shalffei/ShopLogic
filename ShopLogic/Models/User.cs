using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShopLogic.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal MoneyBalance { get; set; }
        public int? AllCountOrders { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<Order> UserOrders { get; set; } = new List<Order>();


    }
}
