using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;
using QuizMasterAPI;
using QuizMasterAPI.Models;

namespace QuizMasterAPI.Controllers
{
    [RoutePrefix("api/user")]
    public class UsersController : ApiController
    {
        private QuizMasterDbContext db = new QuizMasterDbContext();

        // GET: api/Users
        [HttpPost]
        [Route("get")]
        public IQueryable<User> GetUser(User currentUser)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            if ((foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return db.User;
            }
            else
                return null;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        [HttpPost]
        [Route("get/{id}")]
        public IHttpActionResult GetUser(int id,User currentUser)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not authoriZed"));
            }
            User user = db.User.Find(id);
           
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [Route("login")]
        public IHttpActionResult Login(User user)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(user.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            else if (foundUser != null && user.UserPass.Equals(foundUser.UserPass))
            {
                if (foundUser.UserType == "admin")
                {
                    //Return admin
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Accepted, "admin"));
                }                 
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Accepted, "presenter"));
            }
            else // When user found but password incorrect
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Ambiguous,"Password Incorrect"));

        }

        // PUT: api/Users/5
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id,JObject jdata)
        {
            dynamic JsonData = jdata;
            User currentUser = JsonData.currentUser.ToObject<User>();
            User user = JsonData.user.ToObject<User>();
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not authoriZed"));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "User Updated"));
        }

        // POST: api/Users
        [Route("")]
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(JObject jdata)
        {
            dynamic JsonData = jdata;
            User currentUser = JsonData.currentUser.ToObject<User>();
            User user = JsonData.user.ToObject<User>();
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized")); ; 
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);
            db.SaveChanges();
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "New User Added"));
        }

        // DELETE: api/Users/5
         [Route("{id}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id, User currentUser)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return null; ;
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.UserId == id) > 0;
        }
    }
}