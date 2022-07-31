namespace KUSYS_Demo.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public User User { get; set; }
        public ICollection<Student_Course> Student_Course { get; set; }
    }
}
