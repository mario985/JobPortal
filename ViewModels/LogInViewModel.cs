
using System.ComponentModel.DataAnnotations;

public class LogInViewModel
{
    [Required]
    [Display(Name ="username or email")]
    public string EmailOrUsername { set; get; }
    [Required]
    public string Password { set; get; }
    public bool RememberMe{ set; get; }
}