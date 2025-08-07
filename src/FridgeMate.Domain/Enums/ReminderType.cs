namespace FridgeMate.Domain.Enums;

/// <summary>
/// 提醒类型枚举
/// </summary>
public enum ReminderType
{
    /// <summary>
    /// 即将过期
    /// </summary>
    Expiring,
    
    /// <summary>
    /// 已过期
    /// </summary>
    Expired,
    
    /// <summary>
    /// 库存不足
    /// </summary>
    LowStock
} 