using FridgeMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FridgeMate.Infrastructure.Data.Configurations;

/// <summary>
/// 食材实体配置
/// </summary>
public class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.ToTable("food_items");
        
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(f => f.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(f => f.Quantity)
            .HasColumnName("quantity")
            .HasColumnType("DECIMAL(10,2)")
            .IsRequired();
        
        builder.Property(f => f.Unit)
            .HasColumnName("unit")
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(f => f.AddedDate)
            .HasColumnName("added_date")
            .IsRequired();
        
        builder.Property(f => f.ExpiryDate)
            .HasColumnName("expiry_date")
            .IsRequired();
        
        builder.Property(f => f.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasDefaultValue(Domain.Enums.FoodStatus.Normal)
            .IsRequired();
        
        builder.Property(f => f.Notes)
            .HasColumnName("notes")
            .HasColumnType("TEXT");
        
        builder.Property(f => f.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        
        builder.Property(f => f.UpdatedAt)
            .HasColumnName("updated_at");
        
        builder.Property(f => f.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();
        
        // 添加约束
        builder.HasCheckConstraint("chk_quantity", "quantity > 0");
        builder.HasCheckConstraint("chk_expiry_date", "expiry_date > added_date");
        
        // 添加索引
        builder.HasIndex(f => f.Status).HasDatabaseName("idx_food_items_status");
        builder.HasIndex(f => f.ExpiryDate).HasDatabaseName("idx_food_items_expiry");
        builder.HasIndex(f => f.Name).HasDatabaseName("idx_food_items_name");
    }
} 