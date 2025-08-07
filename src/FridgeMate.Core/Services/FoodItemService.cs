using AutoMapper;
using FridgeMate.Core.DTOs;
using FridgeMate.Core.Exceptions;
using FridgeMate.Domain.Entities;
using FridgeMate.Domain.Enums;
using FridgeMate.Infrastructure.Caching;
using FridgeMate.Infrastructure.Repositories;
using FridgeMate.Shared.Constants;
using FridgeMate.Shared.Models;

namespace FridgeMate.Core.Services;

/// <summary>
/// 食材服务实现
/// </summary>
public class FoodItemService : IFoodItemService
{
    private readonly IFoodItemRepository _foodItemRepository;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;

    public FoodItemService(IFoodItemRepository foodItemRepository, ICacheService cacheService, IMapper mapper)
    {
        _foodItemRepository = foodItemRepository;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FoodItemDto>> GetAllAsync()
    {
        var foodItems = await _foodItemRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FoodItemDto>>(foodItems);
    }

    public async Task<FoodItemDto?> GetByIdAsync(Guid id)
    {
        var foodItem = await _foodItemRepository.GetByIdAsync(id);
        return _mapper.Map<FoodItemDto>(foodItem);
    }

    public async Task<PagedResult<FoodItemDto>> GetPagedAsync(int pageNumber, int pageSize, string? name = null, FoodStatus? status = null)
    {
        var pagedResult = await _foodItemRepository.GetPagedAsync(pageNumber, pageSize, name, status);
        var dtos = _mapper.Map<IEnumerable<FoodItemDto>>(pagedResult.Items);
        
        return new PagedResult<FoodItemDto>
        {
            Items = dtos,
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };
    }

    public async Task<FoodItemDto> CreateAsync(CreateFoodItemRequest request)
    {
        var foodItem = new FoodItem
        {
            Name = request.Name,
            Quantity = request.Quantity,
            Unit = request.Unit,
            AddedDate = request.AddedDate,
            ExpiryDate = request.ExpiryDate,
            Notes = request.Notes,
            Status = CalculateStatus(request.ExpiryDate)
        };

        await _foodItemRepository.AddAsync(foodItem);
        
        // 清除相关缓存
        await ClearRelatedCache();
        
        return _mapper.Map<FoodItemDto>(foodItem);
    }

    public async Task<FoodItemDto> UpdateAsync(Guid id, CreateFoodItemRequest request)
    {
        var existingFoodItem = await _foodItemRepository.GetByIdAsync(id);
        if (existingFoodItem == null)
            throw new NotFoundException($"食材 {id} 不存在");

        existingFoodItem.Name = request.Name;
        existingFoodItem.Quantity = request.Quantity;
        existingFoodItem.Unit = request.Unit;
        existingFoodItem.AddedDate = request.AddedDate;
        existingFoodItem.ExpiryDate = request.ExpiryDate;
        existingFoodItem.Notes = request.Notes;
        existingFoodItem.Status = CalculateStatus(request.ExpiryDate);

        await _foodItemRepository.UpdateAsync(existingFoodItem);
        
        // 清除相关缓存
        await ClearRelatedCache();
        
        return _mapper.Map<FoodItemDto>(existingFoodItem);
    }

    public async Task DeleteAsync(Guid id)
    {
        var foodItem = await _foodItemRepository.GetByIdAsync(id);
        if (foodItem == null)
            throw new NotFoundException($"食材 {id} 不存在");

        await _foodItemRepository.DeleteAsync(id);
        
        // 清除相关缓存
        await ClearRelatedCache();
    }

    public async Task<IEnumerable<FoodItemDto>> GetExpiringFoodsAsync()
    {
        // 尝试从缓存获取
        var cacheKey = CacheKeys.GetFullKey(CacheKeys.ExpiringFoods);
        var cachedResult = await _cacheService.GetAsync<IEnumerable<FoodItemDto>>(cacheKey);
        
        if (cachedResult != null)
            return cachedResult;

        // 从数据库获取
        var expiringFoods = await _foodItemRepository.GetExpiringFoodsAsync();
        var dtos = _mapper.Map<IEnumerable<FoodItemDto>>(expiringFoods);
        
        // 缓存结果（15分钟）
        await _cacheService.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(15));
        
        return dtos;
    }

    /// <summary>
    /// 计算食材状态
    /// </summary>
    /// <param name="expiryDate">过期时间</param>
    /// <returns>食材状态</returns>
    private static FoodStatus CalculateStatus(DateTime expiryDate)
    {
        var now = DateTime.UtcNow;
        var timeUntilExpiry = expiryDate - now;
        
        if (timeUntilExpiry.TotalHours <= 0)
            return FoodStatus.Expired;
        else if (timeUntilExpiry.TotalHours <= AppConstants.ExpiringThresholdHours)
            return FoodStatus.NearlyExpired;
        else
            return FoodStatus.Normal;
    }

    /// <summary>
    /// 清除相关缓存
    /// </summary>
    private async Task ClearRelatedCache()
    {
        await _cacheService.RemoveAsync(CacheKeys.GetFullKey(CacheKeys.ExpiringFoods));
        await _cacheService.RemoveAsync(CacheKeys.GetFullKey(CacheKeys.FoodStats));
    }
} 