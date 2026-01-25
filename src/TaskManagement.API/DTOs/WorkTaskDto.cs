using TaskManagement.Domain.Enums;

namespace TaskManagement.API.DTOs;

public record WorkTaskDto(
    int Id,
    string Title,
    string? Description,
    WorkTaskStatus Status,
    int CategoryId,
    string? CategoryName,
    DateTimeOffset CreatedDate,
    DateTimeOffset UpdatedDate
);
