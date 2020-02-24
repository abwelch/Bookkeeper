using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookkeeper.Models;
using Microsoft.AspNetCore.Identity;
using Bookkeeper.Data;

namespace Bookkeeper.Controllers
{
    // This controller processes all account functionality 
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly BookkeeperDbContext db;

        public AccountController(UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager,
            BookkeeperDbContext _db)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            db = _db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerInput)
        {
            if (!ModelState.IsValid)
            {
                return View(registerInput);
            }
            IdentityUser newUser = new IdentityUser
            {
                UserName = registerInput.Username,
                Email = registerInput.Email,
            };
            // This overload of CreateAsync will automatically hash the password argument
            var result = await userManager.CreateAsync(newUser, registerInput.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            // If CreateAsync failed, document errors before returning to register page
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerInput);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginInput)
        {
            if (!ModelState.IsValid)
            {
                return View(loginInput);
            }
            var result = await signInManager.PasswordSignInAsync(loginInput.Username, loginInput.Password,
                false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(loginInput);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}