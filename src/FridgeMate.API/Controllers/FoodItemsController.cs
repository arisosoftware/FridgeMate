using FridgeMate.Core.DTOs;
using FridgeMate.Core.Services;
using FridgeMate.Domain.Enums;
using FridgeMate.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace FridgeMate.API.Controllers;

/// <summary>
/// 食材管理API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FoodItemsController : ControllerBase
{
    private readonly IFoodItemService _foodItemService;

    public FoodItemsController(IFoodItemService foodItemService)
    {
        _foodItemService = foodItemService;
    }

    /// <summary>
    /// 获取所有食材
    /// </summary>
    /// <returns>食材列表</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems()
    {
        var foodItems = await _foodItemService.GetAllAsync();
        return Ok(foodItems);
    }

    /// <summary>
    /// 根据ID获取食材
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <returns>食材详情</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<FoodItemDto>> GetFoodItem(Guid id)
    {
        var foodItem = await _foodItemService.GetByIdAsync(id);
        if (foodItem == null)
            return NotFound();

        return Ok(foodItem);
    }

    /// <summary>
    /// 分页获取食材
    /// </summary>
    /// <param name="pageNumber">页码</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="name">名称筛选</param>
    /// <param name="status">状态筛选</param>
    /// <returns>分页结果</returns>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<FoodItemDto>>> GetFoodItemsPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? name = null,
        [FromQuery] FoodStatus? status = null)
    {
        var result = await _foodItemService.GetPagedAsync(pageNumber, pageSize, name, status);
        return Ok(result);
    }

    /// <summary>
    /// 创建食材
    /// </summary>
    /// <param name="request">创建请求</param>
    /// <returns>创建的食材</returns>
    [HttpPost]
    public async Task<ActionResult<FoodItemDto>> CreateFoodItem([FromBody] CreateFoodItemRequest request)
    {
        var foodItem = await _foodItemService.CreateAsync(request);
        return CreatedAtAction(nameof(GetFoodItem), new { id = foodItem.Id }, foodItem);
    }

    /// <summary>
    /// 更新食材
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <param name="request">更新请求</param>
    /// <returns>更新后的食材</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<FoodItemDto>> UpdateFoodItem(Guid id, [FromBody] CreateFoodItemRequest request)
    {
        try
        {
            var foodItem = await _foodItemService.UpdateAsync(id, request);
            return Ok(foodItem);
        }
        catch (Core.Exceptions.NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// 删除食材
    /// </summary>
    /// <param name="id">食材ID</param>
    /// <returns>无内容</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFoodItem(Guid id)
    {
        try
        {
            await _foodItemService.DeleteAsync(id);
            return NoContent();
        }
        catch (Core.Exceptions.NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// 获取即将过期的食材
    /// </summary>
    /// <returns>即将过期的食材列表</returns>
    [HttpGet("expiring")]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetExpiringFoods()
    {
        var expiringFoods = await _foodItemService.GetExpiringFoodsAsync();
        return Ok(expiringFoods);
    }
} 