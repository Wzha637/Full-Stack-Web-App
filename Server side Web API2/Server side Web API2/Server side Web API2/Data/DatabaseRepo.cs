using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace A2.Data
{
    public class DatabaseRepo : IDatabaseRepo
    {
        private readonly DataBase _dbContext;
        public DatabaseRepo(DataBase dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CheckRegister(string UserName)
        {
            if (_dbContext.Users.Any(e => e.UserName == UserName))
            {
                return true;
            } else
            {
                return false;
            }
        }
        public User Register(string userName, string password, string address)
        {
            User user = new User { UserName = userName, Password = password, Address = address };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }
        public bool ValidLogin(string userName, string password)
        {
            User user = _dbContext.Users.FirstOrDefault(e => e.UserName == userName && e.Password == password);
            if (user == null)
                return false;
            else
                return true;
        }
        public Order PurchaseItem(string UserName, int ProductID, int Quantity)// method to purchase an item
        {
            Order order = new Order { UserName = UserName, ProductID = ProductID, Quantity = Quantity };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
            return order;
        }

    }
}
