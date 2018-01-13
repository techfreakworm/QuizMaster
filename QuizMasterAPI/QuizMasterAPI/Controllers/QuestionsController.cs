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
        [Route("{currentUser}")]
        // GET: api/Questions
        public IQueryable<Question> GetQuestions(User currentUser)
        {
            if (!(currentUser.UserType == "admin" && currentUser.UserPass == db.User.Find(currentUser.UserName).UserPass))
            {
                return null;
            }
            return db.Questions;
        }

        // GET: api/Questions/5
        //[ResponseType(typeof(Question))]
        //public IHttpActionResult GetQuestion(int id)
        //{
        //    Question question = db.Questions.Find(id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(question);
        //}

        // PUT: api/Questions/5
        [Route("{id}/{question}/{currentUser}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuestion(int id, Question question, User currentUser)
        {
            if (!(currentUser.UserType == "admin" && currentUser.UserPass == db.User.Find(currentUser.UserName).UserPass))
            {
                return null; ;
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
        [Route("{question}/{currentUser}")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult PostQuestion(Question question, User currentUser)
        {
            if (!(currentUser.UserType == "admin" && currentUser.UserPass == db.User.Find(currentUser.UserName).UserPass))
            {
                return null; ;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Questions.Add(question);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = question.QId }, question);
        }

        // DELETE: api/Questions/5
        [Route("{id}/{currentUser}")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult DeleteQuestion(int id, User currentUser)
        {
            if (!(currentUser.UserType == "admin" && currentUser.UserPass == db.User.Find(currentUser.UserName).UserPass))
            {
                return null; ;
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            db.Questions.Remove(question);
            db.SaveChanges();

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.QId == id) > 0;
        }
        [HttpGet]
        [Route("random/{currentUser}")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult RandomQuestion(User currentUser)
        {
            if (!((currentUser.UserType == "presenter" || currentUser.UserType == "admin") && currentUser.UserPass == db.User.Find(currentUser.UserName).UserPass))
            {
                return null; ;
            }
            int total = db.Questions.Count();
            int id = rnd.Next(0, total);
            int counter = 0;
            while (ignoreRand.Contains(id))
            {
                if (counter < total)
                    counter++;
                else
                    return null;
                id = rnd.Next(0, total);
            }
            Question question = db.Questions.OrderBy(x => x.QId).Skip(id).FirstOrDefault();
            ignoreRand.Add(id);
            return Ok(question);
        }
    }
}