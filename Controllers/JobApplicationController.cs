using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class JobApplicationController : Controller
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IJobRepository _jobRepository;
    public JobApplicationController(IJobApplicationRepository jobApplicationRepository, IJobRepository jobRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _jobRepository = jobRepository;


    }
    [HttpGet]
    [Authorize(Roles = "User")]
    public IActionResult Apply(int jobId)
    {
        
        JobApplication application
       = new JobApplication
       {
           JobId = jobId

       };
        return View(application);

    }
    [HttpPost]
    [Authorize(Roles = "User")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Apply(JobApplicationViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        if (ModelState.IsValid)
        {

            var jobApplication = new JobApplication
            {
                JobId = model.JobId,
                UserId =userId,
                CoverLetter = model.coverLetter,
                Status = JobApplication.ApplicationStatus.pending
            };
            if (model.cvFile != null && model.cvFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.cvFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cv", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.cvFile.CopyToAsync(stream);
                }
                jobApplication.ResumeUrl = "/uploads/cv/" + fileName;
            }
            else
            {
                ModelState.AddModelError("CvFile", "CV file is required.");
                return View(model);
            }

            await _jobApplicationRepository.AddAsync(jobApplication);
            return RedirectToAction("UserApplications");
        }

        return View(model);
    }


    public async Task<IActionResult> Details(int jobId, string userId)
    {
        if (jobId <= 0 || string.IsNullOrEmpty(userId))
        {
            return NotFound("Invalid job ID or user ID");
        }

        var application = await _jobApplicationRepository.Details(jobId, userId);
        if (application == null)
        {
            return NotFound("Application not found");
        }
        var job = await _jobRepository.GetByIdAsync(application.JobId);
        JobApplicationDetailsViewModel jm =
        new JobApplicationDetailsViewModel
        {
            Job = job,
            JobApplication=application
        };
        return View(jm);
    }
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UserApplications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var applications = await _jobApplicationRepository.UserApplications(userId);
        return View(applications);
    }
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> CompanyApplications()
    {
        var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var appliactions = await _jobApplicationRepository.CompanyApplications(companyId);
        return View(appliactions);


    }
    public async Task<IActionResult> UpdateStatus(int jobId , string userId, string status)
    {
        var application = await _jobApplicationRepository.Details(jobId, userId);
         if (application == null)
        {
            return Json(new { success = false, message = "Application not found" });
        }
        if (!Enum.TryParse<JobApplication.ApplicationStatus>(status, true, out var parsedStatus))
        {
            return Json(new { success = false, message = "Invalid status" });
        }
        application.Status = parsedStatus;
        await _jobApplicationRepository.UpdateAsync(application);
        return Json(new { success = true, message = "Application status updated successfully" });
    }


}