using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public bool IsCompany { set; get; }
    [RequiredIfCompany]
    public string? Field { set; get; }
    public string? Address { set; get; }
    public ICollection<JobApplication>?JobApplications{ set; get; }


    
}