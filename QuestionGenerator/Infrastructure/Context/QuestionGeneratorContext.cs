using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Domain.Entities;

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
        public DbSet<Token> Tokens => Set<Token>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Assessment>().ToTable("Assessments")
                .HasQueryFilter(x => !x.IsDeleted)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<Document>().ToTable("Documents")
                .HasQueryFilter(x => !x.IsDeleted)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<Option>().ToTable("Options")
                .HasQueryFilter(x => !x.IsDeleted)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<Question>().ToTable("Questions")
                .HasQueryFilter(x => !x.IsDeleted)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<AssesmentSubmission>().ToTable("AssessmentSubmissions")
                .HasQueryFilter(x => !x.IsDeleted)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<QuestionResult>().ToTable("Results")
                .HasQueryFilter(x => !x.IsDeleted)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<Role>().ToTable("Roles")
                .HasQueryFilter(x => !x.IsDeleted)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<User>().ToTable("Users")
                .HasQueryFilter(x => !x.IsDeleted)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<Token>().ToTable("Tokens")
                .HasQueryFilter(x => !x.IsDeleted && x.Expires >= DateTime.UtcNow)
                .Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<Role>().HasData(
                new Role { CreatedBy = "1", DateCreated = new DateTime(2024, 6, 27), Id = 1, Name = "Basic User" },
                new Role { CreatedBy = "1", DateCreated = new DateTime(2024, 6, 27), Id = 2, Name = "Premium User" }
                );

        }
    }
}
