using QuizMasterAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMasterAPI.Tests
{
    class TestQuestionDbSet : TestDbSet<Question>
    {
        public override Question Find(params object[] keyValues)
        {
            return this.SingleOrDefault(Question => Question.QId == (int)keyValues.Single());
        }

    }
}
