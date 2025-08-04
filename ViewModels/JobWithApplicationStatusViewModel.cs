using JobPortal.Models;

namespace JobPortal.ViewModels
{
    public class JobWithApplicationStatusViewModel
    {
        public Job Job { get; set; }
        public bool HasApplied { get; set; }
        public JobApplication.ApplicationStatus? ApplicationStatus { get; set; }
    }
} 