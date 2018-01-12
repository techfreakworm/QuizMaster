using QuizMasterAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QuizMasterAPI
{
    public class QuizMasterDbContext :DbContext
    {
        public QuizMasterDbContext()
           : base("QuizMasterDbConnection")
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Team>().ToTable("Teams");

            base.OnModelCreating(modelBuilder);
        }
    }
}