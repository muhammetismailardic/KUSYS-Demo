namespace KUSYS_Demo.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<Student_Course> Student_Courses { get; set; }
    }
}
