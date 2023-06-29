using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using A2.Data;
using System.Net.Http.Headers;
using System.Text;
using A2.Model;
using System.Security.Claims;

namespace A2.Handler
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>//derive the class defined in using Microsoft.AspNetCore.Authentication;
    {
        private readonly IDatabaseRepo _repository;//reference to the data store is kept for carrying authentication checks

        public AuthenticationHandler(//constructor
            IDatabaseRepo repository,//represents data repository layer
            IOptionsMonitor<AuthenticationSchemeOptions> options,//must have these four parameters
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)// call the constructor of the parent class
        {
            _repository = repository;//reference to the data store is kept for carrying authentication checks
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()//must override this method defined in the super class.
        {

            if (!Request.Headers.ContainsKey("Authorization"))//check if the user has sent the credentials of the user 
            {
                Response.Headers.Add("WWW-Authenticate", "Basic");//holds the infomation that will be sent back to the client
                                                                  // it includes the authentication header to tell the client authentication is needed.
                return AuthenticateResult.Fail("Authorization header not found.");//tells caller(server container) the authentication has failed.
            }
            else
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);//extract the value in the autherization header.eg: "Basic ......(base 64 encoding)"
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);// obtian the base 64 encoding of the hearder's parameter by storing it in a byte array.
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");// convert the bytes(UTF8 encoding) to string
                var username = credentials[0];//store the username
                var password = credentials[1];// store the password

                if (_repository.ValidLogin(username, password))// check if the username/password exist in a record in the Users table
                {
                    var claims = new[] { new Claim("userName", username) };// create a claim

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");// create claims identity
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);// create claims principal

                    AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);// create authentication ticket

                    return AuthenticateResult.Success(ticket);//return authentication ticket to service container
                }
                else
                    return AuthenticateResult.Fail("userName and password do not match");// authentication fails tell the caller
            }
        }
    }
}
