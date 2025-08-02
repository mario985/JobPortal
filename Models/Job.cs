using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using JobPortal.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
public class Job
{
    public int Id { set; get; }
    [Required]
    [MaxLength(20, ErrorMessage = "title length shouldn't be more than 20 words")]
    [MinLength(4, ErrorMessage = "title length cant be less than 4 words")]
    public string Title { set; get; }
    [Required]
    [MaxLength(1000, ErrorMessage = "description length shouldn't be more than 1000 characters")]
    public string Description { set; get; }
    [ForeignKey("ApplicationUser")]
    public string CompanyId { set; get; }
     [ValidateNever]
    public ApplicationUser Company { set; get; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "salary cant be a negative number")]
    public int Salary { set; get; }
    [Required]
    [Range(2, 12, ErrorMessage = "the workhours should be higher than 1 and less than 13 hours")]
    public int WorkHours { set; get; }
    [Required]
    public JobType Type { set; get; }
    public enum JobType
    {
        FullTime,
        PartTime,
        Internship

    }
    [ValidateNever]
    public ICollection<JobApplication> JobApplications { set; get; }
    [Required]
    public string Location { set; get; }
    public DateTime CreatedAt { set; get; } = DateTime.UtcNow;
    
    
}