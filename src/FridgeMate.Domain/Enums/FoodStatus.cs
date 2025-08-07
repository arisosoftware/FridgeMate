namespace FridgeMate.Domain.Enums;

/// <summary>
/// 食材状态枚举
/// </summary>
public enum FoodStatus
{
    /// <summary>
    /// 正常（距离过期 > 48小时）
    /// </summary>
    Normal,
    
    /// <summary>
    /// 即将过期（距离过期 ≤ 48小时）
    /// </summary>
    NearlyExpired,
    
    /// <summary>
    /// 已过期（已超过过期时间）
    /// </summary>
    Expired
} 