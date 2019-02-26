using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Internship_Application.Models
{
    public partial class CSCI476Context : DbContext
    {
        public CSCI476Context()
        {
        }

        public CSCI476Context(DbContextOptions<CSCI476Context> options)
            : base(options)
        {
        }

        public virtual DbSet<EmployerDetails> EmployerDetails { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<OfficeDetails> OfficeDetails { get; set; }
        public virtual DbSet<Signatures> Signatures { get; set; }
        public virtual DbSet<StudentDetails> StudentDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost; Database=CSCI476; User Id=sa; Password=4060761Ant");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<EmployerDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessLiscense)
                    .IsRequired()
                    .HasColumnName("business_liscense")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.InternshipEndDate)
                    .HasColumnName("internship_end_date")
                    .HasColumnType("date");

                entity.Property(e => e.InternshipHoursPerWeek).HasColumnName("internship_hours_per_week");

                entity.Property(e => e.InternshipIsPaid).HasColumnName("internship_is_paid");

                entity.Property(e => e.InternshipNumWeeks).HasColumnName("internship_num_weeks");

                entity.Property(e => e.InternshipStartDate)
                    .HasColumnName("internship_start_date")
                    .HasColumnType("date");

                entity.Property(e => e.IntershipAdditionalCompensation)
                    .IsRequired()
                    .HasColumnName("intership_additional_compensation")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.IntershipPaidAmount)
                    .IsRequired()
                    .HasColumnName("intership_paid_amount")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasColumnName("org_name")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionsComments)
                    .IsRequired()
                    .HasColumnName("questions_comments")
                    .HasColumnType("text");

                entity.Property(e => e.QuestionsInternOutcome)
                    .IsRequired()
                    .HasColumnName("questions_intern_outcome")
                    .HasColumnType("text");

                entity.Property(e => e.QuestionsInternProjects)
                    .IsRequired()
                    .HasColumnName("questions_intern_projects")
                    .HasColumnType("text");

                entity.Property(e => e.QuestionsInternRole)
                    .IsRequired()
                    .HasColumnName("questions_intern_role")
                    .HasColumnType("text");

                entity.Property(e => e.SiteVisitAvailable).HasColumnName("site_visit_available");

                entity.Property(e => e.StateIssued)
                    .IsRequired()
                    .HasColumnName("state_issued")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.SupervisorEmail)
                    .IsRequired()
                    .HasColumnName("supervisor_email")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.SupervisorName)
                    .IsRequired()
                    .HasColumnName("supervisor_name")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.SupervisorPhone)
                    .IsRequired()
                    .HasColumnName("supervisor_phone")
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.Property(e => e.SupervisorTitle)
                    .IsRequired()
                    .HasColumnName("supervisor_title")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmployerDetailsId).HasColumnName("employer_details_id");

                entity.Property(e => e.EmployerId)
                    .IsRequired()
                    .HasColumnName("employer_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FacultyId)
                    .IsRequired()
                    .HasColumnName("faculty_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OfficeDetailsId).HasColumnName("office_details_id");

                entity.Property(e => e.SignaturesId).HasColumnName("signatures_id");

                entity.Property(e => e.StudentDetailsId).HasColumnName("student_details_id");

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasColumnName("student_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<OfficeDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Classes)
                    .IsRequired()
                    .HasColumnName("classes")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.CorrespondenceSentEmployer).HasColumnName("correspondence_sent_employer");

                entity.Property(e => e.CorrespondenceSentStudent).HasColumnName("correspondence_sent_student");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateReceived)
                    .HasColumnName("date_received")
                    .HasColumnType("date");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.EstMidPoint)
                    .IsRequired()
                    .HasColumnName("est_mid_point")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Gpa).HasColumnName("gpa");

                entity.Property(e => e.MeetsClassReq).HasColumnName("meets_class_req");

                entity.Property(e => e.MeetsGpaReq).HasColumnName("meets_gpa_req");

                entity.Property(e => e.MeetsPosReq).HasColumnName("meets_pos_req");

                entity.Property(e => e.Other)
                    .IsRequired()
                    .HasColumnName("other")
                    .HasColumnType("text");

                entity.Property(e => e.Pos)
                    .IsRequired()
                    .HasColumnName("pos")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Signatures>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.DirectorSignDate)
                    .HasColumnName("director_sign_date")
                    .HasColumnType("date");

                entity.Property(e => e.DirectorSigned).HasColumnName("director_signed");

                entity.Property(e => e.EmployerSignDate)
                    .HasColumnName("employer_sign_date")
                    .HasColumnType("date");

                entity.Property(e => e.EmployerSigned).HasColumnName("employer_signed");

                entity.Property(e => e.FacultySignDate)
                    .HasColumnName("faculty_sign_date")
                    .HasColumnType("date");

                entity.Property(e => e.FacultySigned).HasColumnName("faculty_signed");

                entity.Property(e => e.StudentServicesSignDate)
                    .HasColumnName("student_services_sign_date")
                    .HasColumnType("date");

                entity.Property(e => e.StudentServicesSigned).HasColumnName("student_services_signed");

                entity.Property(e => e.StudentSignDate)
                    .HasColumnName("student_sign_date")
                    .HasColumnType("date");

                entity.Property(e => e.StudentSigned).HasColumnName("student_signed");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<StudentDetails>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.GraduationYear)
                    .IsRequired()
                    .HasColumnName("graduation_year")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.LeagallyAuth).HasColumnName("leagally_auth");

                entity.Property(e => e.Major)
                    .IsRequired()
                    .HasColumnName("major")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.SemesterYear)
                    .IsRequired()
                    .HasColumnName("semester_year")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasColumnName("student_id")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });
        }
    }
}
