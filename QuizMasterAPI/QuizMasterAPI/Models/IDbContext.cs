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
        DbSet<Question> Questions { get;  }
        DbSet<User> User { get;  }
        DbSet<Team> Teams { get;  }
        int SaveChanges();
        void MarkAsModified(Question item);
        void MarkAsModified(User item);
        void MarkAsModified(Team item);
    }
}
