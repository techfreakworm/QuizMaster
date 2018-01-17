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
using Newtonsoft.Json.Linq;
using QuizMasterAPI;
using QuizMasterAPI.Models;

namespace QuizMasterAPI.Controllers
{
    [RoutePrefix("api/team")]
    public class TeamsController : ApiController
    {
        private QuizMasterDbContext db = new QuizMasterDbContext();

        // GET: api/Teams
        [HttpPost]
        [Route("get")]
        public IQueryable<Team> GetTeams(User currentUser)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            if (!((foundUser.UserType.Equals("admin") || (foundUser.UserType.Equals("presenter"))) && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return null;
            }
            return db.Teams;
        }

        // GET: api/Teams/5
        [HttpPost]
        [Route("get/{id}")]
        [ResponseType(typeof(Team))]
        public IHttpActionResult GetTeam(int id,User currentUser)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized"));
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        // PUT: api/Teams/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTeam(int id, JObject jdata)
        {
            dynamic JsonData = jdata;
            User currentUser = JsonData.currentUser.ToObject<User>();
            Team team = JsonData.team.ToObject<Team>();
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized"));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.TeamId)
            {
                return BadRequest();
            }

            db.Entry(team).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!TeamExists(id))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Name already exists"));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Process your request"));
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Teams
        [Route("")]
        [ResponseType(typeof(Team))]
        public IHttpActionResult PostTeam(JObject jdata)
        {
            dynamic JsonData = jdata;
            User currentUser = JsonData.currentUser.ToObject<User>();
            Team team = JsonData.team.ToObject<Team>();
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized"));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Teams.Add(team);
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

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "New Team Added"));
        
        }

        // DELETE: api/Teams/5
        [Route("{id}")]
        [ResponseType(typeof(Team))]
        public IHttpActionResult DeleteTeam(int id,User currentUser)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized"));
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }

            try
            {
                db.Teams.Remove(team);
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

            return Ok(team);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(int id)
        {
            return db.Teams.Count(e => e.TeamId == id) > 0;
        }

    }
}