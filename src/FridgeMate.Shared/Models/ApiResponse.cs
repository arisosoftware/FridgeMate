namespace FridgeMate.Shared.Models;

/// <summary>
/// API响应模型
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// 错误代码
    /// </summary>
    public string? ErrorCode { get; set; }
    
    /// <summary>
    /// 创建成功响应
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="message">消息</param>
    /// <returns>API响应</returns>
    public static ApiResponse<T> CreateSuccess(T data, string message = "操作成功")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
    
    /// <summary>
    /// 创建失败响应
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="errorCode">错误代码</param>
    /// <returns>API响应</returns>
    public static ApiResponse<T> CreateFailure(string message, string? errorCode = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };
    }
} 