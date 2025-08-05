using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        string[] roles = { "User", "Company" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
        var companyUser = new ApplicationUser
        {

            UserName = "Company123",
            Email = "Company@jobPortal.com",
            Field = "technology",
            IsCompany = true,
            Address = "cairo,egypt"

        };
        var userfound = await userManager.FindByEmailAsync(companyUser.Email);
        if (userfound == null)
        {
            var result = await userManager.CreateAsync(companyUser, "Company123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(companyUser, "Company");
            }

        }
               var user = new ApplicationUser
        {

            UserName = "User123",
            Email = "User@jobPortal.com",
            IsCompany = false,
            Address = "cairo,egypt"

        };
        var found = await userManager.FindByEmailAsync(user.Email);
        if (found == null)
        {
              var result1 = await userManager.CreateAsync(user, "User1234");
            if (result1.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                Console.WriteLine("True");
        }
            
        }

    }
}