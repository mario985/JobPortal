using System.ComponentModel.DataAnnotations;

public class UpdateAccountViewModel
{
    public string Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required, EmailAddress]
    public string Email { get; set; }
    
    public string Address { get; set; }
}