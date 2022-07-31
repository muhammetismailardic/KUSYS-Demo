namespace KUSYS_Demo.Models.ViewModel
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
