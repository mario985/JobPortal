using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [RedirectIfAuthenticated]
    public IActionResult RegisterAsCompany()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
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
    [RedirectIfAuthenticated]
    public IActionResult RegisterAsUser()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAsUser(UserRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                Address = model.Address,
                UserName = model.Name,
                IsCompany = false
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
    [RedirectIfAuthenticated]
    public IActionResult LogIn()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogIn(LogInViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailOrUsername);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.EmailOrUsername);
            }
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
               user.UserName,
               model.Password,
               model.RememberMe,
               lockoutOnFailure: false
           );
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Job");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account is locked ");
                }

            }
        }
        ModelState.AddModelError("", "email or password is not correct");
        return View(model);


    }
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("LogIn");
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UpdateAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            UpdateAccountViewModel updateAccountViewModel
             = new UpdateAccountViewModel
             {
                 Id = user.Id,
                 Name = user.UserName,
                 Email = user.Email,
                 Address = user.Address

             };
            return View(updateAccountViewModel);
        }

        return NotFound();

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]

    public async Task<IActionResult> UpdateAccount(UpdateAccountViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }
            else
            {
                user.Email = vm.Email;
                user.Address = vm.Address;
                user.UserName = vm.Name;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var identityErrors = result.Errors.Select(e => e.Description).ToArray();
                    return Json(new { success = false, identityErrors });
                }
                return Json(new { success = true, message = "Account updated successfully" });
            }

        }
        var errors = ModelState
            .Where(x => x.Value.Errors.Any())
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return Json(new { success = false, errors });



    }
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Json(new { success = false, message = "User not found" });
        }
        
        if (ModelState.IsValid)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return Json(new { success = true, message = "Password changed successfully!" });
            }
            else
            {
                var identityErrors = result.Errors.Select(e => e.Description).ToArray();
                return Json(new { success = false, identityErrors });
            }
        }
        
        var errors = ModelState
            .Where(x => x.Value.Errors.Any())
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return Json(new { success = false, errors });
    }



}
