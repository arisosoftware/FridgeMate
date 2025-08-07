namespace FridgeMate.Core.DTOs;

/// <summary>
/// 创建食材请求
/// </summary>
public class CreateFoodItemRequest
{
    public string Name { get; set; } = string.Empty;
    public float Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string? Notes { get; set; }
} 