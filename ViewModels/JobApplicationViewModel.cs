    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
public class JobApplicationViewModel
{
    [ForeignKey("Job")]
    public int JobId { set; get; }
    [ValidateNever]
    public Job Job { set; get; }
    [ForeignKey("ApplicationUser")]
    public string UserId { set; get; }
    [ValidateNever]
    public ApplicationUser User { set; get; }
    [Required]
    public IFormFile cvFile{ set; get; }
    public string coverLetter{ set; get; }
}