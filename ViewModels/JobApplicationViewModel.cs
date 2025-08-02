    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
public class JobApplicationViewModel
{
    [Required]
    public int JobId { set; get; }  
    [Required]
    public string UserId { set; get; }
    [Required]
    public IFormFile cvFile{ set; get; }
    public string coverLetter{ set; get; }
}