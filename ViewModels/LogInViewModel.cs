
using System.ComponentModel.DataAnnotations;

public class LogInViewModel
{
    [Required]
    public string Email { set; get; }
    [Required]
    public string Password { set; get; }
    public bool RememberMe{ set; get; }
}