using FridgeMate.Core.DTOs;
using FridgeMate.Domain.Enums;
using FridgeMate.Shared.Models;

namespace FridgeMate.Core.Services;

/// <summary>
/// 食材服务接口
/// </summary>
public interface IFoodItemService
{
    /// <summary>
    /// 获取所有食材
    /// </summary>
    /// <returns>食材列表</returns>
    Task<IEnumerable<FoodItemDto>> GetAllAsync();
    
    /// <summary>
    /// 根据ID获取食材
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <returns>食材</returns>
    Task<FoodItemDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// 分页获取食材
    /// </summary>
    /// <param name="pageNumber">页码</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="name">名称筛选</param>
    /// <param name="status">状态筛选</param>
    /// <returns>分页结果</returns>
    Task<PagedResult<FoodItemDto>> GetPagedAsync(int pageNumber, int pageSize, string? name = null, FoodStatus? status = null);
    
    /// <summary>
    /// 创建食材
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的食材</returns>
    Task<FoodItemDto> CreateAsync(CreateFoodItemRequest request);
    
    /// <summary>
    /// 更新食材
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新后的食材</returns>
    Task<FoodItemDto> UpdateAsync(Guid id, CreateFoodItemRequest request);
    
    /// <summary>
    /// 删除食材
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <returns>任务</returns>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// 获取即将过期的食材
    /// </summary>
    /// <returns>即将过期的食材列表</returns>
    Task<IEnumerable<FoodItemDto>> GetExpiringFoodsAsync();
} 