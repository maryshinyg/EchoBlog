using EchoBlog.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace EchoBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var identityUser = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };
            var identityResult = await _userManager.CreateAsync(identityUser, model.Password);
            if (identityResult.Succeeded)
            {
                //assign this user to "user" role
                var roleIdentityResult = await _userManager.AddToRoleAsync(identityUser, "user");
                if (roleIdentityResult.Succeeded)
                {
                    //show success notification
                    return RedirectToAction("Register");
                }
            }
            //show error notification
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if(signInResult != null && signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            //show error notification
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}