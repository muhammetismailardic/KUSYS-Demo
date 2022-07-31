using System.ComponentModel.DataAnnotations;

namespace KUSYS_Demo.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required, MaxLength(256), Display(Name = "Username or Email Address")]
        public string UsernameOrEmail { get; set; }

        [Required, MinLength(6), MaxLength(32), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool IsValid()
        {
            if (UsernameOrEmail == null)
            {
                return false;
            }

            if (Password == null)
            {
                return false;
            }

            return true;
        }
    }
}
