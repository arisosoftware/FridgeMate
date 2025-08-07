namespace FridgeMate.Domain.Entities;

/// <summary>
/// 菜谱食材关系实体
/// </summary>
public class RecipeIngredient
{
    public Guid RecipeId { get; set; }
    public Guid IngredientId { get; set; }
    public float Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    // 导航属性
    public virtual Recipe Recipe { get; set; } = null!;
    public virtual FoodItem Ingredient { get; set; } = null!;
} 