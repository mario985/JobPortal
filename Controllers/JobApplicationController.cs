using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class JobApplicationController : Controller
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    public JobApplicationController(IJobApplicationRepository jobApplicationRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;

    }
    [HttpGet]
    [Authorize(Roles ="User")]
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
    [Authorize(Roles ="User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(JobApplication jobApplication)
    {
        if (ModelState.IsValid)
        {
            await _jobApplicationRepository.AddAsync(jobApplication);
        }
        return RedirectToAction("Index", "Job");
    }

    public async Task<IActionResult> Details(int jobId, string userId)
    {
        var application = await _jobApplicationRepository.Details(jobId, userId);
        return View(application);


    }
    [Authorize(Roles ="User")]
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
        var appliactions =await _jobApplicationRepository.CompanyApplications(companyId);
        return View(appliactions);


    }


}