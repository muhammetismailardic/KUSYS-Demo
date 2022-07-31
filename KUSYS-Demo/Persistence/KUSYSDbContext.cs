using KUSYS_Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KUSYS_Demo.Persistence
{
    public class KUSYSDBContext : IdentityDbContext<User, Role, int>
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public DbSet<Student_Course> Student_Courses { get; set; }

        public KUSYSDBContext(DbContextOptions<KUSYSDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Setting initial model behaviours.
            BuildUserModel(modelBuilder);
            BuildStudentModel(modelBuilder);
            BuildCourseModel(modelBuilder);
            BuildStudent_CourseModel(modelBuilder);
        }

        private void BuildStudent_CourseModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student_Course>()
                .HasOne(b => b.Student)
                .WithMany(ba => ba.Student_Course)
                .HasForeignKey(bi => bi.StudentId);

            modelBuilder.Entity<Student_Course>()
              .HasOne(b => b.Course)
              .WithMany(ba => ba.Student_Courses)
              .HasForeignKey(bi => bi.CourseId);
        }
        private void BuildUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(32);

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(32);

            });
        }
        private void BuildStudentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId)
                      .ValueGeneratedOnAdd();
                
                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(32);

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(32);

                entity.Property(e => e.BirthDate)
                      .HasMaxLength(32);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime2");

                entity.HasOne(d => d.User)
                     .WithMany(p => p.Students)
                     .HasForeignKey(d => d.UserId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Student_User");

            });
        }
        private void BuildCourseModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.CourseId)
                      .IsRequired()
                      .HasMaxLength(32);

                entity.Property(e => e.CourseName)
                      .IsRequired()
                      .HasMaxLength(32);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime2");
            });
        }
    }
}
