using FridgeMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FridgeMate.Infrastructure.Data.Configurations;

/// <summary>
/// 菜谱食材关系实体配置
/// </summary>
public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.ToTable("recipe_ingredients");
        
        builder.HasKey(ri => new { ri.RecipeId, ri.IngredientId });
        
        builder.Property(ri => ri.RecipeId)
            .HasColumnName("recipe_id")
            .IsRequired();
        
        builder.Property(ri => ri.IngredientId)
            .HasColumnName("ingredient_id")
            .IsRequired();
        
        builder.Property(ri => ri.Quantity)
            .HasColumnName("quantity")
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();
        
        builder.Property(ri => ri.Unit)
            .HasColumnName("unit")
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(ri => ri.Notes)
            .HasColumnName("notes")
            .HasColumnType("TEXT");
        
        // 配置外键关系
        builder.HasOne(ri => ri.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ri => ri.Ingredient)
            .WithMany(f => f.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // 添加约束
        builder.HasCheckConstraint("chk_ingredient_quantity", "quantity > 0");
        
        // 添加索引
        builder.HasIndex(ri => ri.RecipeId).HasDatabaseName("idx_recipe_ingredients_recipe");
        builder.HasIndex(ri => ri.IngredientId).HasDatabaseName("idx_recipe_ingredients_ingredient");
    }
} 