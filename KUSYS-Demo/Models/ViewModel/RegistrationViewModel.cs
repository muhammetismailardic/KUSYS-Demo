using System.ComponentModel.DataAnnotations;

namespace KUSYS_Demo.Models.ViewModel
{
    public class RegistrationViewModel
    {
        [Required, EmailAddress, MaxLength(256), Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, MaxLength(256), Display(Name = "Username")]
        public string Username { get; set; }

        [Required, MaxLength(256), Display(Name = "Name")]
        public string FirstName { get; set; }

        [Required, MinLength(6), MaxLength(32), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Required, MinLength(6), MaxLength(32), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsValid()
        {
            if (Email == null)
            {
                return false;
            }

            if (Username == null)
            {
                return false;
            }

            if (Password == null)
            {
                return false;
            }

            if (ConfirmPassword == null)
            {
                return false;
            }

            if (Password != ConfirmPassword)
            {
                return false;
            }
            return true;
        }
    }
}
