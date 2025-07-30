using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult RegisterAscompany()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterAsCompany(CompanyRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                Address = model.Address,
                UserName = model.Name,
                Field = model.field,
                IsCompany = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Company");
                return View("LogIn");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }

        return View(model);
    }
    [HttpGet]
    public IActionResult RegisterAsUser()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterAsUser(UserRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                Address = model.Address,
                UserName = model.Name,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return RedirectToAction("LogIn");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }

        return View(model);

    }
    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure : false
        );
        if (result.Succeeded)
        {
            return RedirectToAction("Home", "Index");
        }
        else if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "Your account is locked ");
        }
        ModelState.AddModelError("", "username or password is not correct");
        return View(model);


    }
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("LogIn");
    }


}
