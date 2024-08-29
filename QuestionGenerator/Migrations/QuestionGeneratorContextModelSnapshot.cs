﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuestionGenerator.Infrastructure.Context;

#nullable disable

namespace QuestionGenerator.Migrations
{
    [DbContext(typeof(QuestionGeneratorContext))]
    partial class QuestionGeneratorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Assessment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AssessmentType")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DifficultyLevel")
                        .HasColumnType("int");

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("UserId");

                    b.ToTable("Assessments", (string)null);
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.AssessmentSubmission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AssessmentId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentId");

                    b.HasIndex("DocumentId");

                    b.HasIndex("UserId");

                    b.ToTable("AssessmentSubmissions", (string)null);
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DocumentUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("TableOfContentsJson")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Documents", (string)null);
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("OptionText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Options", (string)null);
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("AssessmentId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Elucidation")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentId");

                    b.ToTable("Questions", (string)null);
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.QuestionResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AssessmentSubmissionId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("UserAnswer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentSubmissionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Results", (string)null);
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = "1",
                            DateCreated = new DateTime(2024, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            Name = "Free User"
                        },
                        new
                        {
                            Id = 2,
                            CreatedBy = "1",
                            DateCreated = new DateTime(2024, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            Name = "Premium User"
                        });
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<int>("TokenType")
                        .HasColumnType("int");

                    b.Property<string>("TokenValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens", (string)null);
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GoogleId")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Assessment", b =>
                {
                    b.HasOne("QuestionGenerator.Core.Domain.Entities.Document", "Document")
                        .WithMany("Assessments")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestionGenerator.Core.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.AssessmentSubmission", b =>
                {
                    b.HasOne("QuestionGenerator.Core.Domain.Entities.Assessment", "Assessment")
                        .WithMany("AssessmentSubmissions")
                        .HasForeignKey("AssessmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestionGenerator.Core.Domain.Entities.Document", "Document")
                        .WithMany("AssessmentSubmissions")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestionGenerator.Core.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assessment");

                    b.Navigation("Document");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Document", b =>
                {
                    b.HasOne("QuestionGenerator.Core.Domain.Entities.User", "User")
                        .WithMany("Documents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Option", b =>
                {
                    b.HasOne("QuestionGenerator.Core.Domain.Entities.Question", "Question")
                        .WithMany("Options")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Question", b =>
                {
                    b.HasOne("QuestionGenerator.Core.Domain.Entities.Assessment", "Assessment")
                        .WithMany("Questions")
                        .HasForeignKey("AssessmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assessment");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.QuestionResult", b =>
                {
                    b.HasOne("QuestionGenerator.Core.Domain.Entities.AssessmentSubmission", "AssesmentSubmission")
                        .WithMany("Results")
                        .HasForeignKey("AssessmentSubmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestionGenerator.Core.Domain.Entities.Question", "Question")
                        .WithMany("Results")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssesmentSubmission");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Token", b =>
                {
                    b.HasOne("QuestionGenerator.Core.Domain.Entities.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.User", b =>
                {
                    b.HasOne("QuestionGenerator.Core.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Assessment", b =>
                {
                    b.Navigation("AssessmentSubmissions");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.AssessmentSubmission", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Document", b =>
                {
                    b.Navigation("AssessmentSubmissions");

                    b.Navigation("Assessments");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Question", b =>
                {
                    b.Navigation("Options");

                    b.Navigation("Results");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("QuestionGenerator.Core.Domain.Entities.User", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
