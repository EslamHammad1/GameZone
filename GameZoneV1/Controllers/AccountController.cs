
using Microsoft.AspNetCore.Identity;

namespace GameZoneV1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager , SignInManager<IdentityUser> signInManager)
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
        public async Task<IActionResult> Register(RegisterViewModel newModel) 
        {
            if (ModelState.IsValid)
            {
                IdentityUser Model = new IdentityUser();
                Model.UserName = newModel.UserName;
                Model.Email = newModel.Email;
                Model.PasswordHash = newModel.Password;

                IdentityResult result = await _userManager.CreateAsync(Model, newModel.Password);
                if (result.Succeeded == true)
                {
                    await _signInManager.SignInAsync(Model, false);
                   return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(newModel);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel UserLogin)
        {
            if (ModelState.IsValid)
            {
                IdentityUser userModel = await _userManager.FindByNameAsync(UserLogin.UserName) ?? throw new InvalidOperationException("Username invalid");
                if (userModel != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(userModel , UserLogin.Password);
                    if (found )
                    {
                        await _signInManager.SignInAsync(userModel, UserLogin.rememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", " Username And Password invalid");

            }
            return View(UserLogin);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
