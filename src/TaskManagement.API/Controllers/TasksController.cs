using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(TaskContext context) : ControllerBase
{
    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkTaskDto>>> GetTasks()
    {
        // Best Practice: AsNoTracking for read-only queries improves performance
        // Best Practice: Eager Loading (Include) to avoid N+1 problem
        var tasks = await context.WorkTasks
            .AsNoTracking()
            .Include(t => t.Category)
            .Select(t => new WorkTaskDto(
                t.Id,
                t.Title,
                t.Description,
                t.Status,
                t.CategoryId,
                t.Category != null ? t.Category.Name : null,
                EF.Property<DateTimeOffset>(t, "CreatedDate"),
                EF.Property<DateTimeOffset>(t, "UpdatedDate")
            ))
            .ToListAsync();

        return Ok(tasks);
    }

    // GET: api/tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<WorkTaskDto>> GetTask(int id)
    {
        var task = await context.WorkTasks
            .AsNoTracking()
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(new WorkTaskDto(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.CategoryId,
            task.Category?.Name,
            EF.Property<DateTimeOffset>(task, "CreatedDate"),
            EF.Property<DateTimeOffset>(task, "UpdatedDate")
        ));
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<WorkTaskDto>> CreateTask(CreateTaskDto createDto)
    {
        // Verify category exists
        var category = await context.Categories.FindAsync(createDto.CategoryId);
        if (category == null)
        {
            return BadRequest("Invalid Category ID.");
        }

        var task = new WorkTask
        {
            Title = createDto.Title,
            Description = createDto.Description,
            Status = createDto.Status,
            CategoryId = createDto.CategoryId
        };

        context.WorkTasks.Add(task);
        await context.SaveChangesAsync();

        // Get shadow properties
        var entry = context.Entry(task);
        var createdDate = entry.Property<DateTimeOffset>("CreatedDate").CurrentValue;
        var updatedDate = entry.Property<DateTimeOffset>("UpdatedDate").CurrentValue;

        var dto = new WorkTaskDto(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.CategoryId,
            category.Name,
            createdDate,
            updatedDate
        );

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, dto);
    }

    // PUT: api/tasks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updateDto)
    {
        var task = await context.WorkTasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Verify category if changed
        if (task.CategoryId != updateDto.CategoryId)
        {
             var categoryExists = await context.Categories.AnyAsync(c => c.Id == updateDto.CategoryId);
             if (!categoryExists) return BadRequest("Invalid Category ID.");
        }

        task.Title = updateDto.Title;
        task.Description = updateDto.Description;
        task.Status = updateDto.Status;
        task.CategoryId = updateDto.CategoryId;

        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/tasks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await context.WorkTasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // Soft Delete
        task.IsArchived = true;
        await context.SaveChangesAsync();

        return NoContent();
    }
}
