using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.Model;

namespace A2.Data
{
    public interface IDatabaseRepo
    {
        bool CheckRegister(string UserName);//check if the client has already registered
        User Register(string userName, string password, string address);//register the client
        public bool ValidLogin(string userName, string password);//defined for checking a valid login

        Order PurchaseItem(string UserName, int ProductID, int Quantity);// method to purchase an item

    }
}
