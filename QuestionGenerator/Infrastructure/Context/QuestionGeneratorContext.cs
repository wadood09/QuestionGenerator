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
        public DbSet<RevisitedAssesment> RevistedAssesments => Set<RevisitedAssesment>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Assessment>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Document>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Option>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Question>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<RevisitedAssesment>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property<int>("Id").ValueGeneratedOnAdd();

            modelBuilder.Entity<Assessment>().ToTable(nameof(Assessment));
            modelBuilder.Entity<Document>().ToTable(nameof(Document));
            modelBuilder.Entity<Option>().ToTable(nameof(Option));
            modelBuilder.Entity<Question>().ToTable(nameof(Question));
            modelBuilder.Entity<RevisitedAssesment>().ToTable(nameof(RevisitedAssesment));
            modelBuilder.Entity<Role>().ToTable(nameof(Role));
            modelBuilder.Entity<User>().ToTable(nameof(User));

            modelBuilder.Entity<Role>().HasData(
                new Role { CreatedBy = "1", DateCreated = new DateTime(2024, 6, 27), Id = 1, Name = "Basic User" },
                new Role { CreatedBy = "1", DateCreated = new DateTime(2024, 6, 27), Id = 2, Name = "Premium User" }
                );

        }
    }
}
