using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Internship_Application.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Forms> Forms { get; set; }
        public virtual DbSet<StatusCodes> StatusCodes { get; set; }
        public virtual DbSet<Templates> Templates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=Data;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Forms>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdministratorQuestions)
                    .IsRequired()
                    .HasColumnName("administrator_questions")
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('{}')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmployerQuestions)
                    .IsRequired()
                    .HasColumnName("employer_questions")
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('{}')");

                entity.Property(e => e.FacultyQuestions)
                    .IsRequired()
                    .HasColumnName("faculty_questions")
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('{}')");

                entity.Property(e => e.StudentQuestions)
                    .IsRequired()
                    .HasColumnName("student_questions")
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('{}')");

                entity.Property(e => e.StudentServicesQuestions)
                    .IsRequired()
                    .HasColumnName("student_services_questions")
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('{}')");

                entity.Property(e => e.TemplateId).HasColumnName("template_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WuId)
                    .IsRequired()
                    .HasColumnName("wu_id")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusCodes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Details)
                    .IsRequired()
                    .HasColumnName("details")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.StatusCode)
                    .IsRequired()
                    .HasColumnName("status_code")
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Templates>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Disclaimer)
                    .IsRequired()
                    .HasColumnName("disclaimer")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasColumnName("display_name")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsRetired).HasColumnName("is_retired");

                entity.Property(e => e.Questions)
                    .IsRequired()
                    .HasColumnName("questions")
                    .IsUnicode(false)
                    .HasDefaultValueSql("('[{\"Prompt\":\"\", \"InputType\": \"\", \"HelperText\": \"\", \"Order\": \"\", \"Role\": \"\", \"Required\": \"\", \"ProcessQuestion\": \"\", \"DateSigned\": \"\", \"Options\": \"[]\"}]')");

                entity.Property(e => e.RetiredAt)
                    .HasColumnName("retired_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.TemplateName)
                    .IsRequired()
                    .HasColumnName("template_name")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });
        }
    }
}
