using QuizMasterAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMasterAPI.Tests
{
    class TestDbContext : IDbContext

    {
        public TestDbContext()
        {
            this.Questions = new TestQuestionDbSet();
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<User> User { get ; set ; }
        public DbSet<Team> Teams { get ; set ; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Question item) { }
        public void Dispose() { }

        public void MarkAsModified(User item)
        {
            
        }

        public void MarkAsModified(Team item)
        {
            
        }
    }
}
