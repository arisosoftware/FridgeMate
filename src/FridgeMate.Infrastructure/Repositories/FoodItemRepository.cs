using FridgeMate.Domain.Entities;
using FridgeMate.Domain.Enums;
using FridgeMate.Infrastructure.Data;
using FridgeMate.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FridgeMate.Infrastructure.Repositories;

/// <summary>
/// 食材仓储实现
/// </summary>
public class FoodItemRepository : IFoodItemRepository
{
    private readonly FridgeMateDbContext _context;

    public FoodItemRepository(FridgeMateDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FoodItem>> GetAllAsync()
    {
        return await _context.FoodItems
            .Where(f => !f.IsDeleted)
            .OrderBy(f => f.Name)
            .ToListAsync();
    }

    public async Task<FoodItem?> GetByIdAsync(Guid id)
    {
        return await _context.FoodItems
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
    }

    public async Task<PagedResult<FoodItem>> GetPagedAsync(int pageNumber, int pageSize, string? name = null, FoodStatus? status = null)
    {
        var query = _context.FoodItems.Where(f => !f.IsDeleted);

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(f => f.Name.Contains(name));
        }

        if (status.HasValue)
        {
            query = query.Where(f => f.Status == status.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(f => f.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<FoodItem>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<IEnumerable<FoodItem>> GetAvailableFoodsAsync()
    {
        return await _context.FoodItems
            .Where(f => !f.IsDeleted && f.Status != FoodStatus.Expired)
            .OrderBy(f => f.ExpiryDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<FoodItem>> GetExpiringFoodsAsync()
    {
        return await _context.FoodItems
            .Where(f => !f.IsDeleted && f.Status == FoodStatus.NearlyExpired)
            .OrderBy(f => f.ExpiryDate)
            .ToListAsync();
    }

    public async Task AddAsync(FoodItem foodItem)
    {
        foodItem.CreatedAt = DateTime.UtcNow;
        await _context.FoodItems.AddAsync(foodItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FoodItem foodItem)
    {
        foodItem.UpdatedAt = DateTime.UtcNow;
        _context.FoodItems.Update(foodItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var foodItem = await GetByIdAsync(id);
        if (foodItem != null)
        {
            foodItem.IsDeleted = true;
            foodItem.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.FoodItems
            .AnyAsync(f => f.Id == id && !f.IsDeleted);
    }
} 