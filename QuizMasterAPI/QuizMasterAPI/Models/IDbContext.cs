using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMasterAPI.Models
{
    public interface IDbContext : IDisposable
    {
        DbSet<Question> Questions { get; set; }
        DbSet<User> User { get; set; }
        DbSet<Team> Teams { get; set; }
        int SaveChanges();
        void MarkAsModified(Question item);
        void MarkAsModified(User item);
        void MarkAsModified(Team item);
    }
}
