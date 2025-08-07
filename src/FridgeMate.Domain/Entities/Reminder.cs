using FridgeMate.Domain.Enums;

namespace FridgeMate.Domain.Entities;

/// <summary>
/// 提醒实体
/// </summary>
public class Reminder
{
    public Guid Id { get; set; }
    public Guid FoodItemId { get; set; }
    public ReminderType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    
    // 导航属性
    public virtual FoodItem FoodItem { get; set; } = null!;
} 