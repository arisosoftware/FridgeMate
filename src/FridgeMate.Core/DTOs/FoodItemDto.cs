using FridgeMate.Domain.Enums;

namespace FridgeMate.Core.DTOs;

/// <summary>
/// 食材DTO
/// </summary>
public class FoodItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public FoodStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 