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
using QuizMasterAPI.Filters;
using QuizMasterAPI.Models;

namespace QuizMasterAPI.Controllers
{
    
    [RoutePrefix("api/question")]
    [JwtAuthentication]
    public class QuestionsController : ApiController
    {
        //global varibles for question controller
        Random rnd = new Random();
        private static List<int> ignoreRand = new List<int> { }; //list that stores already generated random numbers
        private static int counter = 0;                         // keeps count of random questions generated
        private IDbContext db = new QuizMasterDbContext();
        public QuestionsController() { }
        public QuestionsController(IDbContext context)
        {
            db = context;
        }
        //returns all the questions
        [Route("get")]
        // GET: api/Questions
        [HttpGet]
        public IQueryable<Question> GetQuestions()
        {
            HttpRequestMessage message = this.Request;
            String token = message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return null;
            }
            if (!foundUser.UserType.Equals("admin"))
            { 
                return null;
            }
            return db.Questions;
        }
        // return a question by id
        // GET: api/Questions/5
        [Route("get/{id}")]
        [HttpGet]
        [ResponseType(typeof(Question))]
        public IHttpActionResult GetQuestion(int id)
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
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized"));
            }
            Question question = db.Questions.Find(id);
            
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }
        //update question
        // PUT: api/Questions/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuestion(int id,Question question)
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
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Not Authorized"));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.QId)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Wrong Id"));
            }

            //db.Entry(question).State = EntityState.Modified;
            db.MarkAsModified(question);

            try
            {
                db.SaveChanges();
                return Ok("product Added");
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
        }
        //add new question
        // POST: api/Questions
        [Route("")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult PostQuestion(Question question,String token="abc")
        {
            HttpRequestMessage message = this.Request;
            if (token.Equals("abc"))
            {
                token = message.Headers.Authorization.ToString().Substring(7);
            }
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!foundUser.UserType.Equals("admin"))
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
        // delete a question
        // DELETE: api/Questions/5
        [Route("{id}")]
        [ResponseType(typeof(Question))]
        public IHttpActionResult DeleteQuestion(int id)
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
        // returns a random question everytime
        
        [Route("random")]
        [HttpGet]
        [ResponseType(typeof(Question))]
        public IHttpActionResult RandomQuestion()
        {
            HttpRequestMessage message = this.Request;
            String token = message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!(foundUser.UserType.Equals("admin") || foundUser.UserType.Equals("presenter")))
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
        //check weather the answer is correct or not and updates score accordingly
        [HttpPost]
        [Route("check")]
        public IHttpActionResult CheckAnswer(JObject jdata)
        {
            dynamic JsonData = jdata;
            HttpRequestMessage message = this.Request;
            String token = message.Headers.Authorization.ToString().Substring(7);
            String username = JwtManager.DecodeToken(token);
            User foundUser = db.User.Where(a => a.UserName.Equals(username)).FirstOrDefault();
            int qId;
            int tId;
            String answer;
            int PassFLAG = 0;
            try
            {
                qId = JsonData.qId.ToObject<int>();
                tId = JsonData.tId.ToObject<int>();
                answer = JsonData.answer.ToObject<String>();
                PassFLAG = JsonData.PassFlag.ToObject<int>();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            if (foundUser == null)
            {
                return NotFound();
            }
            if (!foundUser.UserType.Equals("presenter"))
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
                        //db.Entry(team).State = EntityState.Modified;
                        db.MarkAsModified(team);
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
                    team.TeamScore += 5;
                    team.QuestionsPassed++;
                    try
                    {
                        //db.Entry(team).State = EntityState.Modified;
                        db.MarkAsModified(team);
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
                //db.Entry(team).State = EntityState.Modified;
                db.MarkAsModified(team);
                db.SaveChanges();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Wrong Answer"));
            }
            return Ok();
        }
    }
}