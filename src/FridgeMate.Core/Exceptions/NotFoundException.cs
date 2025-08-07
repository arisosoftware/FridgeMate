namespace FridgeMate.Core.Exceptions;

/// <summary>
/// 未找到异常
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
} 