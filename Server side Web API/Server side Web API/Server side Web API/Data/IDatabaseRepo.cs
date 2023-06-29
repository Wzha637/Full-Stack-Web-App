using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A1.Model;

namespace A1.Data
{
    public interface IDatabaseRepo
    {
        IEnumerable<Staff> GetAllStaff();

        Staff GetStaff(int id);

        IEnumerable<Products> GetAllItems();

    }
}
