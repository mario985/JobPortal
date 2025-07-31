using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class JobController : Controller
{
    private readonly IJobRepository _jobRepository;
    public JobController(IJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return View(jobs);
    }
    [HttpGet]
    [Authorize(Roles = "Company")]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    [Authorize(Roles = "Company")]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Add(Job job)
    {
        job.ComapnyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (ModelState.IsValid)
        {
            await _jobRepository.AddAsync(job);
            return RedirectToAction("IndexCompany");
        }
        return View(job);

    }
    [HttpGet]
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> Edit(int id)
    {
        var job = await _jobRepository.GetByIdAsync(id);
        if (job == null || !IsJobOwner(job))
        {
            return Unauthorized();
        }
        return View(job);

    }
    [HttpPost]
    [Authorize(Roles = "Company")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Job job)
    {
        if (!IsJobOwner(job)) return Unauthorized();
        if (ModelState.IsValid)
        {
            await _jobRepository.EditAsync(job);
            return RedirectToAction("IndexCompany");

        }
        else return View(job);


    }
    [HttpPost]
    [Authorize(Roles = "Company")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await _jobRepository.GetByIdAsync(id);
        if (!IsJobOwner(job) || job == null) return Unauthorized();
        await _jobRepository.DeleteAsync(id);
        return Ok();
    }
    [Authorize(Roles = "Company")]
    public async Task<IActionResult> IndexCompany()
    {
        var ComapnyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var jobs = await _jobRepository.GetByCompanyIdAsync(ComapnyId);
        return View(jobs);
    }
    private bool IsJobOwner(Job job)
    {
        var ComapnyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return job.ComapnyId == ComapnyId;

    }
    public async Task<IActionResult> Details(int id)
    {
        var job = await _jobRepository.GetByIdAsync(id);
        if (job == null)
        {
            return NotFound();
        }
        return View(job);
    }
}