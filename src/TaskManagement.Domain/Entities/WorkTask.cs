using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

public class WorkTask
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public WorkTaskStatus Status { get; set; } = WorkTaskStatus.ToDo;
    public bool IsArchived { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
