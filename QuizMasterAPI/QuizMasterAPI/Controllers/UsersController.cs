using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using QuizMasterAPI;
using QuizMasterAPI.Models;

namespace QuizMasterAPI.Controllers
{
    public class UsersController : ApiController
    {
        private QuizMasterDbContext db = new QuizMasterDbContext();

        // GET: api/Users
        public IQueryable<User> GetUser(User currentUser)
        {
            if (currentUser.UserType == "admin" && currentUser.UserPass == db.User.Find(currentUser.UserName).UserPass)
            {
                return db.User;
            }
            else
                return null;
        }

        // GET: api/Users/5
        //[ResponseType(typeof(User))]
        //public IHttpActionResult GetUser(int id)
        //{
        //    User user = db.User.Find(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}
        [HttpGet]
        [ResponseType(typeof(User))]
        public IHttpActionResult Login(String UserName,String Password)
        {
            User user = (User) db.User.Where(a => a.UserName.Equals(UserName));
            if (user == null)
            {
                return NotFound();
            }
            else if(!user.UserPass.Equals(Password))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Password Incorrect"));
            }
            
            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user,User currentUser)
        {
            if (!(currentUser.UserType == "admin" && currentUser.UserPass == db.User.Find(currentUser.UserName).UserPass))
            {
                return null; ;
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user, User currentUser)
        {
            if (!(currentUser.UserType == "admin" && user.UserPass == db.User.Find(currentUser.UserName).UserPass))
            {
                return null; ;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id, User currentUser)
        {
            if (!(currentUser.UserType == "admin" && currentUser.UserPass == db.User.Find(currentUser.UserName).UserPass))
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