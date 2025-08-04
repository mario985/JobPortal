using System.ComponentModel.DataAnnotations;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { set; get; }
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    public string NewPassword { set; get; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string ConfirmPassword { set; get; }

}