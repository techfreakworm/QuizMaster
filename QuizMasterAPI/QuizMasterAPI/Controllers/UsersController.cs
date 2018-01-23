using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Results;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizMasterAPI;
using QuizMasterAPI.Filters;
using QuizMasterAPI.Models;

namespace QuizMasterAPI.Controllers
{
    

    [RoutePrefix("api/user")]
    public class UsersController : ApiController
    {
       
        private QuizMasterDbContext db = new QuizMasterDbContext();
        // GET: api/Users
        // Returns users who are admin
        [HttpPost]
        [JwtAuthentication]
        [Route("getadmin")]
        public IQueryable<User> GetAdmin()
        {
            HttpRequestMessage message = this.Request;
            String token =message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            if (foundUser.UserType.Equals("admin"))
            {
                return db.User.Where(a=> a.UserType.Equals("admin"));
            }
            else
                return null;
        }
        // Returns users who are presenter
        [HttpPost]
        [JwtAuthentication]
        [Route("getpresenter")]
        public IQueryable<User> GetPresenter()
        {
            HttpRequestMessage message = this.Request;
            String token = message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            if (foundUser.UserType.Equals("admin"))
            {
                return db.User.Where(a => a.UserType.Equals("presenter"));
            }
            else
                return null;
        }
        //get a user by its id
        // GET: api/Users/5
        [ResponseType(typeof(User))]
        [HttpPost]
        [Route("get/{id}")]
        [JwtAuthentication]
        public IHttpActionResult GetUser(int id)
        {
            HttpRequestMessage message = this.Request;
            String token = message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!foundUser.UserType.Equals("admin"))
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
        //api for login
        [AllowAnonymous]
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
                    var token = JwtManager.GenerateToken(foundUser.UserName);
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Accepted, token));
                }
                else
                {
                    var token = JwtManager.GenerateToken(foundUser.UserName);
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Accepted, token));
                }
            }
            else // When user found but password incorrect
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Ambiguous,"Password Incorrect"));

        }
        //update user
        // PUT: api/Users/5
        [Route("{id}")]
        [HttpPut]
        [JwtAuthentication]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id,User user)
        {

            HttpRequestMessage message = this.Request;
            String token = message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!foundUser.UserType.Equals("admin"))
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
            catch (DbUpdateException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Name already exists"));
                }
            }
            catch (Exception)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Process your request"));
            }

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "User Updated"));
        }
        //add a new user
        // POST: api/Users

        [Route("")]
        [ResponseType(typeof(User))]
        [JwtAuthentication]
        public IHttpActionResult PostUser(User user)
        {

            HttpRequestMessage message = this.Request;
            String token = message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!foundUser.UserType.Equals("admin"))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized")); ; 
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.User.Add(user);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Name Already exists"));
            }
            catch (Exception)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Process your request"));
            }
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "New User Added"));
        }
        //delete user
        // DELETE: api/Users/5
        [Route("{id}")]
        [ResponseType(typeof(User))]
        [HttpDelete]
        [JwtAuthentication]
        public IHttpActionResult DeleteUser(int id)
        {
            HttpRequestMessage message = this.Request;
            String token = message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!foundUser.UserType.Equals("admin"))
            {
                return null; ;
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                db.User.Remove(user);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {

                throw;
            }
            catch (Exception)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Process your request"));
            }


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