using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class JobApplication
{
    [ForeignKey("Job")]
    public int JobId { set; get; }
    public Job Job { set; get; }
    [ForeignKey("ApplicationUser")]
    public string UserId { set; get; }
    public ApplicationUser User { set; get; }
    public bool? IsAccepted { set; get; }
    [Required]
    public string ResumeUrl { set; get; }
    public ApplicationStatus Status{ set; get; }
    public enum ApplicationStatus
    {
        pending,
        rejected,
        accepted
        
    }
}