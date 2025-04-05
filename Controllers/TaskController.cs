using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Tasks.ToListAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        if (ModelState.IsValid)
        {
            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(task);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, TaskItem task)
    {
        if (id != task.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(task);  // This updates the status and other fields
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(task);
    }
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Toggle status between "In Progress" and "Completed"
        if (task.Status == "In Progress")
        {
            task.Status = "Completed";
        }
        else
        {
            task.Status = "In Progress";
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

  [HttpPost, ActionName("Delete")]
  public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound(); // Or you can redirect to an error page or back to the list
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index)); // Redirect back to the list of tasks
    }
     public IActionResult Privacy()
    {
        return View(); // This will return the Privacy.cshtml view
    }
}
