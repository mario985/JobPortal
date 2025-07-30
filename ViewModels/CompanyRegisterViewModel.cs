using System.ComponentModel.DataAnnotations;

public class CompanyRegisterViewModel
{
    [Required]
    public string Name { set; get; }
    [Required]
    public string Email { set; get; }
    [Required]
    public string Address { set; get; }
    [Required]
    [MinLength(8, ErrorMessage = "Minumum password length is 8 words")]
    public string Password { set; get; }
    [Compare("Password")]
    public string ConfirmPassword { set; get; }
    [Required]
    public string field { set; get; }   
}