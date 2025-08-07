using FridgeMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FridgeMate.Infrastructure.Data.Configurations;

/// <summary>
/// 菜谱实体配置
/// </summary>
public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("recipes");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(r => r.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(r => r.Description)
            .HasColumnName("description")
            .HasColumnType("TEXT");
        
        builder.Property(r => r.Steps)
            .HasColumnName("steps")
            .HasColumnType("TEXT")
            .IsRequired();
        
        builder.Property(r => r.CookingTime)
            .HasColumnName("cooking_time")
            .HasDefaultValue(30)
            .IsRequired();
        
        builder.Property(r => r.Difficulty)
            .HasColumnName("difficulty")
            .HasConversion<string>()
            .HasDefaultValue(Domain.Enums.RecipeDifficulty.Easy)
            .IsRequired();
        
        builder.Property(r => r.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(500);
        
        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        
        builder.Property(r => r.UpdatedAt)
            .HasColumnName("updated_at");
        
        builder.Property(r => r.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();
        
        // 添加索引
        builder.HasIndex(r => r.Name).HasDatabaseName("idx_recipes_name");
        builder.HasIndex(r => r.Difficulty).HasDatabaseName("idx_recipes_difficulty");
    }
} 