using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Data;

public class DataSeeder(TaskContext context)
{
    public void Seed()
    {
        if (context.Categories.Any())
        {
            return;
        }

        var categories = new List<Category>
        {
            new Category { Name = "Work" },
            new Category { Name = "Personal" },
            new Category { Name = "Urgent" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        // Add some sample tasks
        var workCategory = categories.First(c => c.Name == "Work");
        context.WorkTasks.Add(new WorkTask
        {
            Title = "Complete Project Plan",
            Description = "Draft the initial plan for the new module.",
            Category = workCategory,
            Status = WorkTaskStatus.InProgress
        });

        context.SaveChanges();
    }
}
