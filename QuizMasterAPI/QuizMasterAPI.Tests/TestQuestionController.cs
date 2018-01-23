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


namespace QuizMasterAPI.Tests
{
    [TestClass]
    public class TestQuestionController
    {
        [TestMethod]

        public void PostQuestion_ShouldReturnSameProduct()
        {
            var controller = new QuestionsController(new TestDbContext());

            var item = new ;


            var result = controller.PostQuestion(item) as IHttpActionResult;
            var response = new HttpResponseMessage(HttpStatusCode.OK);


            Assert.IsNotNull(result);
            Assert.AreEqual(result,response);
        }


    }
}
