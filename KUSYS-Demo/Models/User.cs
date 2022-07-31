using Microsoft.AspNetCore.Identity;

namespace KUSYS_Demo.Models
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            Students = new HashSet<Student>();
        }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public ICollection<Student> Students { get; set; }
    }
}
