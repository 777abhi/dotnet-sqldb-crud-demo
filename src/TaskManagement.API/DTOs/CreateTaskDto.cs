using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.API.DTOs;

public record CreateTaskDto(
    [Required] string Title,
    string? Description,
    [Required] int CategoryId,
    WorkTaskStatus Status = WorkTaskStatus.ToDo
);
