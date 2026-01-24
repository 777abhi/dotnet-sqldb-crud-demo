using System.Collections.Generic;

namespace TaskManagement.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<WorkTask> WorkTasks { get; set; } = new List<WorkTask>();
}
