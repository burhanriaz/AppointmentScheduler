using AppointmentScheduling.Models;
using AppointmentScheduling.Models.AppDbContext;
using AppointmentScheduling.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _appDbContext;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<ApplicationUser> _signInManager;

        public AccountController(AppDbContext appDbContext, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(loginViewModel.Email);
                    HttpContext.Session.SetString("ssuserName", user.Name);
                   // var username = HttpContext.Session.GetString("ssuserName");
                    return RedirectToAction("Index", "Appointment");
                }
                else
                    ModelState.AddModelError("", "Invalid Login Attempt");
            }
            return View(loginViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (!_roleManager.RoleExistsAsync(Helper.Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Doctor));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Patient));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var newuser = new ApplicationUser()
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    Name = registerViewModel.Name

                };
                var result = await _userManager.CreateAsync(newuser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newuser, registerViewModel.RoleName);

                    if (!User.IsInRole(Helper.Helper.Admin))
                    {
                        await _signInManager.SignInAsync(newuser, isPersistent: false);

                    }
                    else
                    {
                        TempData["newAdminSignUp"] = newuser.Name;
                    }
                    return RedirectToAction("Index", "Appointment");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(registerViewModel);
        }
        [HttpPost] 
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}