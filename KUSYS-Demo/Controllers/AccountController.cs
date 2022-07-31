using KUSYS_Demo.Models;
using KUSYS_Demo.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KUSYS_Demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// To get Login Page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Checks the database for entered user to proceed
        /// </summary>
        /// <param name="usernameOrEmail"></param>
        /// <param name="password"></param>
        /// <param name="rememberme"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string usernameOrEmail, string password, bool rememberme = false)
        {
            var model = new LoginViewModel
            {
                UsernameOrEmail = usernameOrEmail,
                Password = password,
                RememberMe = rememberme
            };

            if (model.IsValid())
            {
                try
                {
                    var userName = model.UsernameOrEmail;

                    if (model.UsernameOrEmail.Contains('@'))
                    {
                        var user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);

                        if (user != null)
                        {
                            userName = user.UserName;
                        }
                    }

                    var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($" {ex.Message} An error occured while logging in a user. Username {0}", model.UsernameOrEmail);
                }
            }
            return View();
        }

        /// <summary>
        /// Redirect to Register page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register newly created user to database
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(string email, string username, string password, string confirmPassword)
        {
            var model = new RegistrationViewModel
            {
                Email = email,
                Username = username,
                FirstName = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            if (model.IsValid())
            {
                try
                {
                    var user = new User { UserName = model.Username, Email = model.Email, FirstName = model.FirstName };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Instructor");
                        //await _signInManager.SignInAsync(user, false);
                        return RedirectToAction(nameof(Login));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($" {ex} An error occured while creating a user. Email: {0}", model.Email);
                }
            }

            return View();
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            _signInManager.SignOutAsync().Wait();
            return View(nameof(Login));
        }
    }
}
