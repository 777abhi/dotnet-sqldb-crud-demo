using System.ComponentModel.DataAnnotations;
using TaskManagement.Domain.Enums;

namespace TaskManagement.API.DTOs;

public record UpdateTaskDto(
    [Required] string Title,
    string? Description,
    WorkTaskStatus Status,
    [Required] int CategoryId
);
