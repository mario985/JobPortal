using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string Field { set; get; }
    [Required]
    public string Address { set; get; }
    public ICollection<JobApplication>JobApplications{ set; get; }


    
}