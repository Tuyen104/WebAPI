using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JWTExample.Models;

namespace JWTExample.Controllers
{
    public class LoginController : ApiController
    {
        JWTExampleEntities db = new JWTExampleEntities();
        [HttpPost]
        public HttpResponseMessage Login(User data)
        {
            var user = db.Users.Where(x => x.UserName.Equals(data.UserName)).SingleOrDefault();
            if (user != null)
            {
                if (user.Password.Equals(data.Password))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(data.UserName));
                }
                else
                {
                   return Request.CreateResponse(HttpStatusCode.Forbidden, "Password is wrong");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "The user is not exist!!!");
            }
        }
        [HttpGet]
        public HttpResponseMessage Validate(string username, string token)
        {
            var user = db.Users.Where(x => x.UserName.Equals(username)).SingleOrDefault();
            if (user != null)
            {
                string usernameToken = TokenManager.ValidateToken(token);
                if (user.UserName.Equals(usernameToken))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "token is wrong");
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Username is not exist");
            }
          
        }
    }
}
