using FridgeMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FridgeMate.Infrastructure.Data;

/// <summary>
/// FridgeMate数据库上下文
/// </summary>
public class FridgeMateDbContext : DbContext
{
    public FridgeMateDbContext(DbContextOptions<FridgeMateDbContext> options) : base(options)
    {
    }
    
    /// <summary>
    /// 食材表
    /// </summary>
    public DbSet<FoodItem> FoodItems { get; set; }
    
    /// <summary>
    /// 菜谱表
    /// </summary>
    public DbSet<Recipe> Recipes { get; set; }
    
    /// <summary>
    /// 菜谱食材关系表
    /// </summary>
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    
    /// <summary>
    /// 提醒表
    /// </summary>
    public DbSet<Reminder> Reminders { get; set; }
    
    /// <summary>
    /// 用户表
    /// </summary>
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 应用实体配置
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FridgeMateDbContext).Assembly);
        
        // 配置复合主键
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });
    }
} 