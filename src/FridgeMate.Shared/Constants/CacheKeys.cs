namespace FridgeMate.Shared.Constants;

/// <summary>
/// 缓存键常量
/// </summary>
public static class CacheKeys
{
    /// <summary>
    /// 推荐菜谱列表缓存键
    /// </summary>
    public const string RecommendedRecipes = "recommended_recipes";
    
    /// <summary>
    /// 即将过期食材列表缓存键
    /// </summary>
    public const string ExpiringFoods = "expiring_foods";
    
    /// <summary>
    /// 常用菜谱缓存键
    /// </summary>
    public const string PopularRecipes = "popular_recipes";
    
    /// <summary>
    /// 食材统计缓存键
    /// </summary>
    public const string FoodStats = "food_stats";
    
    /// <summary>
    /// 获取完整的缓存键
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns>完整的缓存键</returns>
    public static string GetFullKey(string key) => $"{AppConstants.CacheKeyPrefix}{key}";
} 