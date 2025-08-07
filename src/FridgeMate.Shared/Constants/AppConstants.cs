namespace FridgeMate.Shared.Constants;

/// <summary>
/// 应用常量
/// </summary>
public static class AppConstants
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public const string AppName = "FridgeMate";
    
    /// <summary>
    /// 应用版本
    /// </summary>
    public const string AppVersion = "1.0.0";
    
    /// <summary>
    /// 默认分页大小
    /// </summary>
    public const int DefaultPageSize = 20;
    
    /// <summary>
    /// 最大分页大小
    /// </summary>
    public const int MaxPageSize = 100;
    
    /// <summary>
    /// 即将过期时间阈值（小时）
    /// </summary>
    public const int ExpiringThresholdHours = 48;
    
    /// <summary>
    /// 缓存键前缀
    /// </summary>
    public const string CacheKeyPrefix = "fridgemate:";
} 