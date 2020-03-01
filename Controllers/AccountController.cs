using System;
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
        private readonly UserManager<IdentityUserExtended> userManager;
        private readonly SignInManager<IdentityUserExtended> signInManager;
        private readonly BookkeeperContext dbContext;
        private readonly IUserInfoUtils userInfoUtils;

        public AccountController(UserManager<IdentityUserExtended> _userManager,
            SignInManager<IdentityUserExtended> _signInManager,
            BookkeeperContext _dbContext,
            IUserInfoUtils _userInfoUtils)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            dbContext = _dbContext;
            userInfoUtils = _userInfoUtils;
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
            DateTime currentDateTime = DateTime.Now;
            UserInfo newUserInfo = new UserInfo
            {
                AccountCreation = currentDateTime,
                LastActivity = currentDateTime,
                TotalCurrentTransactions = 0,
                TotalStatements = 0
            };
            dbContext.UserInfos.Add(newUserInfo);
            dbContext.SaveChanges();
            // EF Core follows the insert and can now retrieve the auto-generated primary key id
            int newUserID = newUserInfo.UserID;
            IdentityUserExtended newUser = new IdentityUserExtended
            {
                UserName = registerInput.Username,
                Email = registerInput.Email,
                UserInfoID = newUserID
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