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
    [RoutePrefix("api/question")]
    public class QuestionsController : ApiController
    {
        private QuizMasterDbContext db = new QuizMasterDbContext();
        Random rnd = new Random();
        static List<int> ignoreRand = new List<int> { };
        [Route("get")]
        // GET: api/Questions
        [HttpPost]
        public IQueryable<Question> GetQuestions(User currentUser)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            if (!(foundUser.UserType.Equals("admin") && currentUser.UserPass.Equals(foundUser.UserPass)))
            { 
                return null;
            }
            return db.Questions;
        }

        // GET: api/Questions/5
        [Route("get/{id}")]
        [HttpPost]
        [ResponseType(typeof(Question))]
        public IHttpActionResult GetQuestion(int id, User currentUser)
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
            Question question = db.Questions.Find(id);
            
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        // PUT: api/Questions/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuestion(int id,JObject jdata)
        {
            dynamic JsonData = jdata;
            User currentUser = JsonData.currentUser.ToObject<User>();
            Question question = JsonData.question.ToObject<Question>();
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

            if (id != question.QId)
            {
                return BadRequest();
            }

            db.Entry(question).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/Questions
        [Route("")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult PostQuestion(JObject jdata)
        {
            dynamic JsonData = jdata;
            User currentUser = JsonData.currentUser.ToObject<User>();
            Question question = JsonData.question.ToObject<Question>();
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
                db.Questions.Add(question);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {

                throw;
            }

            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "New Question Added"));
        }

        // DELETE: api/Questions/5
        [Route("{id}")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult DeleteQuestion(int id, User currentUser)
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
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            try
            {
                db.Questions.Remove(question);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {

                throw;
            }

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ignoreRand.Clear();
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.QId == id) > 0;
        }
        [HttpGet]
        [Route("random")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult RandomQuestion(User currentUser)
        {
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!((foundUser.UserType.Equals("admin") || (foundUser.UserType.Equals("presenter")) && currentUser.UserPass.Equals(foundUser.UserPass))))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized"));
            }
            int total = db.Questions.Count();
            int rno = rnd.Next(0, total);
            int counter = 0;
            while (ignoreRand.Contains(rno))
            {
                if (counter < total)
                    counter++;
                else
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "No Questions found"));
                rno = rnd.Next(0, total);
            }
            Question question = db.Questions.OrderBy(x => x.QId).Skip(rno).FirstOrDefault();
            ignoreRand.Add(rno);
            return Ok(question);
        }
    }
}