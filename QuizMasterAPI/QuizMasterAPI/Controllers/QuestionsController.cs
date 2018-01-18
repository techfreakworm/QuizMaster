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
        Random rnd = new Random();
        private static List<int> ignoreRand = new List<int> { };
        private static int counter = 0;
        private QuizMasterDbContext db = new QuizMasterDbContext();
       
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
            catch (DbUpdateException)
            {
                if (!QuestionExists(id))
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

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Name Already exists"));
            }
            catch (Exception)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Process your request"));
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

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Delete"));
            }
            catch (Exception)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Process your request"));
            }

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
        [HttpPost]
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
            while (QuestionsController.ignoreRand.Contains(rno))
            {
                if (QuestionsController.counter < total)
                    rno = rnd.Next(0, total);
                else
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NoContent, "No Questions found"));
            }
            Question question = db.Questions.OrderBy(x => x.QId).Skip(rno).FirstOrDefault();
            ignoreRand.Add(rno);
            counter++;
            return Ok(question);
        }
        [HttpPost]
        [Route("check")]
        public IHttpActionResult CheckAnswer(JObject jdata)
        {
            dynamic JsonData = jdata;
            User currentUser;
            int qId;
            int tId;
            String answer;
            int PassFLAG = 0;
            try
            {
                currentUser = JsonData.currentUser.ToObject<User>();
                qId = JsonData.qId.ToObject<int>();
                tId = JsonData.tId.ToObject<int>();
                answer = JsonData.answer.ToObject<String>();
                PassFLAG = JsonData.PassFlag.ToObject<int>();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            User foundUser = db.User.Where(a => a.UserName.Equals(currentUser.UserName)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(((foundUser.UserType.Equals("presenter")) && currentUser.UserPass.Equals(foundUser.UserPass))))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized"));
            }
            Question question = db.Questions.Find(qId);
            Team team = db.Teams.Find(tId);
            if (question == null || team == null)
            {
                return BadRequest();
            }
            else if (question.Answer.Equals(answer))
            {
                team.QuestionsAttempted++;
                team.AnswersCorrect++;
                if (PassFLAG == 0)
                {
                    team.TeamScore += 10;
                    try
                    {
                        db.Entry(team).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Process Your Request"));
                    }
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Correct Answer"));
                }
                else if (PassFLAG == 1)
                {
                    team.AnswersCorrect++;
                    team.TeamScore += 5;
                    team.QuestionsPassed++;
                    try
                    {
                        db.Entry(team).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Cant Process Your Request"));
                    }
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Correct Answer"));
                }

            }
            else
            {
                team.QuestionsAttempted++;
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Wrong Answer"));
            }
            return Ok();
        }
    }
}