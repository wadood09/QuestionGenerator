using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Domain.Entities;
using System.Globalization;

namespace QuestionGenerator.Infrastructure.Context
{
    public class QuestionGeneratorContext : DbContext
    {
        public QuestionGeneratorContext(DbContextOptions<QuestionGeneratorContext> options) : base(options)
        {

        }

        public DbSet<Assessment> Assessments => Set<Assessment>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Option> Options => Set<Option>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<AssesmentSubmission> AssessmentSubmissions => Set<AssesmentSubmission>();
        public DbSet<QuestionResult> Results => Set<QuestionResult>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Assessment>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Document>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Option>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Question>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<AssesmentSubmission>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<QuestionResult>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<Assessment>().ToTable("Assessments");
            modelBuilder.Entity<Document>().ToTable("Documents");
            modelBuilder.Entity<Option>().ToTable("Options");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<AssesmentSubmission>().ToTable("AssessmentSubmissions");
            modelBuilder.Entity<QuestionResult>().ToTable("Results");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<User>().ToTable("USers");

            modelBuilder.Entity<Role>().HasData(
                new Role { CreatedBy = "1", DateCreated = new DateTime(2024, 6, 27), Id = 1, Name = "Basic User" },
                new Role { CreatedBy = "1", DateCreated = new DateTime(2024, 6, 27), Id = 2, Name = "Premium User" }
                );

        }
    }
}
