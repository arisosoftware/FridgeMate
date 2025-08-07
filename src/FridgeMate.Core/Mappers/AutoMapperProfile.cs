using AutoMapper;
using FridgeMate.Core.DTOs;
using FridgeMate.Domain.Entities;

namespace FridgeMate.Core.Mappers;

/// <summary>
/// AutoMapper配置
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // FoodItem映射
        CreateMap<FoodItem, FoodItemDto>();
        CreateMap<CreateFoodItemRequest, FoodItem>();
        
        // Recipe映射（待实现）
        // CreateMap<Recipe, RecipeDto>();
        // CreateMap<CreateRecipeRequest, Recipe>();
        
        // Reminder映射（待实现）
        // CreateMap<Reminder, ReminderDto>();
    }
} 