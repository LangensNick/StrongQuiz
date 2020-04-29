using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StrongQuiz.Models.Models;

namespace StrongQuiz.Web.Data
{
    public class StrongQuizDbContext : IdentityDbContext<ApplicationUser>
    {
        public StrongQuizDbContext(DbContextOptions<StrongQuizDbContext> options)
            : base(options)
        { }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<ScoreQuiz> ScoreQuizzes { get; set; }
        public virtual DbSet<UserScore> UserScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //must voor identity

            modelBuilder.Entity<ScoreQuiz>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationUserId, e.QuizId });



            });
            modelBuilder.Entity<UserScore>(entity =>
            {
                entity.Property(e => e.Id).UseSqlServerIdentityColumn();
            });
        }
    }
}
