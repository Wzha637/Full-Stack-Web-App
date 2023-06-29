using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A1.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace A1.Data
{
    public class DatabaseRepo : IDatabaseRepo
    {
        private readonly DataBase _dbContext;
        public DatabaseRepo(DataBase dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Staff> GetAllStaff()
        {
            IEnumerable<Staff> staffs = _dbContext.AllStaff.ToList<Staff>();
            return staffs;
        }
        public Staff GetStaff(int id)
        {
            Staff staff = _dbContext.AllStaff.FirstOrDefault(e => e.Id == id);
            return staff;
        }
        public IEnumerable<Products> GetAllItems()
        {
            IEnumerable<Products> products = _dbContext.AllProducts.ToList<Products>();
            return products;
        }

    }
}
