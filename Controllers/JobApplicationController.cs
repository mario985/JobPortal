using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class JobApplicationController : Controller
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    public JobApplicationController(IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;

    }
    [HttpGet]
    [Authorize(Roles = "User")]
    public IActionResult Apply(int jobId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        JobApplication application
       = new JobApplication
       {
           UserId = userId,
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
        if (ModelState.IsValid)
        {
            var jobApplication = new JobApplication
            {
                JobId = model.JobId,
                UserId = model.UserId,
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
        var application = await _jobApplicationRepository.Details(jobId, userId);
        return View(application);


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
    public async Task<IActionResult> UpdateStatus(int jobId, string userId, string status)
    {
        var application = await _jobApplicationRepository.Details(jobId, userId);
        if (application == null)
        {
            return NotFound();
        }
        return Ok();
    }


}