using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using A2.Data;
using A2.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using A2.Dtos;

namespace A2.Controllers
{
    [Route("api")]
    [ApiController]
    public class Controller1 : Controller
    {
        private readonly IDatabaseRepo _repository;

        public Controller1(IDatabaseRepo repository)
        {
            _repository = repository;//create reference to the data storage
        }
        [HttpPost("Register")]
        public ActionResult Register(RegisterInput user)
        {
            if (_repository.CheckRegister(user.UserName))//true if username already in database
            {
                return Ok("Username not available.");
            }
            else
            {
                _repository.Register(user.UserName, user.Password, user.Address);//add user to database
                return Ok("User successfully registered.");
            }
         
        }
        [Authorize(AuthenticationSchemes = "MyAuthentication")]// name of authentication scheme defined in the startup class
                                                               // indicates that authentication is needed for accessing this method
        [Authorize(Policy = "UserOnly")]// specify the authorization policy to be used after the client gets their ticket, it is defined in startup class which is the ppolicy name.
        [HttpGet("GetVersionA")]

        public ActionResult GetVersionA()
        {
            return Ok("v1");//return version
        }


        [Authorize(AuthenticationSchemes = "MyAuthentication")]// name of authentication scheme defined in the startup class
                                                               // indicates that authentication is needed for accessing this method
        [Authorize(Policy = "UserOnly")]// specify the authorization policy to be used after the client gets their ticket, it is defined in startup class which is the ppolicy name.
        [HttpPost("PurchaseItem")]

        public ActionResult PurchaseItem(PurchaseItemInput item)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();//find first claim given to the user in the ticket
            Claim c = ci.FindFirst("UserName");// find first claim with a key "username" from claimsidentity
            string UserName = c.Value;//retrieve the value of the claim, ie userName
            Order order = _repository.PurchaseItem(UserName, item.ProductID, item.Quantity);//PurchaseItem method will return an Order object that has been saved into the database
            return Ok(order);//return the order 
        }


        [Authorize(AuthenticationSchemes = "MyAuthentication")]// name of authentication scheme defined in the startup class
                                                               // indicates that authentication is needed for accessing this method
        [Authorize(Policy = "UserOnly")]// specify the authorization policy to be used after the client gets their ticket, it is defined in startup class which is the ppolicy name.
        [HttpGet("PurchaseSingleItem/{id}")]

        public ActionResult PurchaseSingleItem(int id)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();//find first claim given to the user in the ticket
            Claim c = ci.FindFirst("UserName");// find first claim with a key "username" from claimsidentity
            string UserName = c.Value;//retrieve the value of the claim, ie userName
            Order order = _repository.PurchaseItem(UserName, id, 1);//PurchaseItem method will return an Order object that has been saved into the database, it has a given product ID and only quantity of 1
            return Ok(order);//return the order 
        }
    }
}
