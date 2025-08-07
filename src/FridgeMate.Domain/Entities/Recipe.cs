using FridgeMate.Domain.Enums;

namespace FridgeMate.Domain.Entities;

/// <summary>
/// 菜谱实体
/// </summary>
public class Recipe
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Steps { get; set; } = string.Empty;
    public int CookingTime { get; set; } // 分钟
    public RecipeDifficulty Difficulty { get; set; } = RecipeDifficulty.Easy;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    
    // 导航属性
    public virtual ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
} 