using FridgeMate.Domain.Enums;

namespace FridgeMate.Domain.Entities;

/// <summary>
/// 食材实体
/// </summary>
public class FoodItem
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
    public bool IsDeleted { get; set; }
    
    // 导航属性
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
} 