using FridgeMate.Domain.Entities;
using FridgeMate.Domain.Enums;
using FridgeMate.Shared.Models;

namespace FridgeMate.Infrastructure.Repositories;

/// <summary>
/// 食材仓储接口
/// </summary>
public interface IFoodItemRepository
{
    /// <summary>
    /// 获取所有食材
    /// </summary>
    /// <returns>食材列表</returns>
    Task<IEnumerable<FoodItem>> GetAllAsync();
    
    /// <summary>
    /// 根据ID获取食材
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <returns>食材</returns>
    Task<FoodItem?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// 分页获取食材
    /// </summary>
    /// <param name="pageNumber">页码</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="name">名称筛选</param>
    /// <param name="status">状态筛选</param>
    /// <returns>分页结果</returns>
    Task<PagedResult<FoodItem>> GetPagedAsync(int pageNumber, int pageSize, string? name = null, FoodStatus? status = null);
    
    /// <summary>
    /// 获取可用食材（未删除且未过期）
    /// </summary>
    /// <returns>可用食材列表</returns>
    Task<IEnumerable<FoodItem>> GetAvailableFoodsAsync();
    
    /// <summary>
    /// 获取即将过期的食材
    /// </summary>
    /// <returns>即将过期的食材列表</returns>
    Task<IEnumerable<FoodItem>> GetExpiringFoodsAsync();
    
    /// <summary>
    /// 添加食材
    /// </summary>
    /// <param name="foodItem">食材</param>
    /// <returns>任务</returns>
    Task AddAsync(FoodItem foodItem);
    
    /// <summary>
    /// 更新食材
    /// </summary>
    /// <param name="foodItem">食材</param>
    /// <returns>任务</returns>
    Task UpdateAsync(FoodItem foodItem);
    
    /// <summary>
    /// 删除食材
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <returns>任务</returns>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// 检查食材是否存在
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(Guid id);
} 