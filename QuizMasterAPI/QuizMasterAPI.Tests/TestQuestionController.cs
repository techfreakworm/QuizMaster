using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizMasterAPI.Controllers;
using QuizMasterAPI.Models;
using Newtonsoft.Json;
using System.Web.Http.Results;
using System.Net.Http.Headers;
using System.Collections;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace QuizMasterAPI.Tests
{
    [TestClass]
    public class TestQuestionController
    {

        [TestMethod]
        public async System.Threading.Tasks.Task PostQuestion_ShouldReturnStatusCodeOk()
        {
            var controller = new QuestionsController(new TestDbContext());
            var token = JwtManager.GenerateToken("admin@cygrp.com");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new { Ques = "how to do aatribute routing in c#", OptionOne = "a", OptionTwo = "b", OptionThree = "c", OptionFour = "d", Answer = "a" };
            var result = await client.PostAsJsonAsync("http://localhost:50827/api/question", content);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task PostSameQuestion_ShouldReturnStatusCodeForbidden()
        {
            var controller = new QuestionsController(new TestDbContext());
            var token = JwtManager.GenerateToken("admin@cygrp.com");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new { Ques = "how to do aatribute routing in c#", OptionOne = "a", OptionTwo = "b", OptionThree = "c", OptionFour = "d", Answer = "a" };
            var result = await client.PostAsJsonAsync("http://localhost:50827/api/question", content);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task PutQuestion_ShouldReturnStatusCodeOk()
        {
            var controller = new QuestionsController(new TestDbContext());
            var token = JwtManager.GenerateToken("admin@cygrp.com");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new { Ques = "how to do aatribute routing in c#", OptionOne = "a", OptionTwo = "b", OptionThree = "c", OptionFour = "d", Answer = "a", QId = 16 };
            var result = await client.PutAsJsonAsync("http://localhost:50827/api/question/" + 16, content);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task PutQuestionWrongId_ShouldReturnStatusCodeForbidden()
        {
            var controller = new QuestionsController(new TestDbContext());
            var token = JwtManager.GenerateToken("admin@cygrp.com");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new { Ques = "how to do aatribute routing in c#", OptionOne = "a", OptionTwo = "b", OptionThree = "c", OptionFour = "d", Answer = "a", QId = 17 };
            var result = await client.PutAsJsonAsync("http://localhost:50827/api/question/" + 16, content);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task GetQuestions_ShouldReturnAllQuestions()
        {
            var context = new TestDbContext();
            var controller = new QuestionsController(context);
            var token = JwtManager.GenerateToken("admin@cygrp.com");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = client.GetAsync("http://localhost:50827/api/question/get").Result;
            var result = await response.Content.ReadAsStringAsync();
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            List<Question> questions = JSserializer.Deserialize<List<Question>>(result);
            Assert.IsNotNull(questions);
            Assert.AreEqual(3, questions.Count());
        }
        [TestMethod]
        public async System.Threading.Tasks.Task GetQuestion_ShouldReturnQuestionsWithSameId()
        {
            var context = new TestDbContext();
            
            var controller = new QuestionsController(context);
            var token = JwtManager.GenerateToken("admin@cygrp.com");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync("http://localhost:50827/api/question/get/17") as HttpResponseMessage;
            var result = response.Content.ReadAsStringAsync().Result;
            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            Question question = JSserializer.Deserialize<Question>(result);
            Assert.IsNotNull(question);
            Assert.AreEqual(17, question.QId);
        }
    }
}
