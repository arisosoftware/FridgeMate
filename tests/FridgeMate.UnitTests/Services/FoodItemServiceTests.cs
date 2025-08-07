using AutoMapper;
using FridgeMate.Core.DTOs;
using FridgeMate.Core.Services;
using FridgeMate.Domain.Entities;
using FridgeMate.Domain.Enums;
using FridgeMate.Infrastructure.Caching;
using FridgeMate.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace FridgeMate.UnitTests.Services;

/// <summary>
/// 食材服务单元测试
/// </summary>
public class FoodItemServiceTests
{
    private readonly Mock<IFoodItemRepository> _mockRepository;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly FoodItemService _service;

    public FoodItemServiceTests()
    {
        _mockRepository = new Mock<IFoodItemRepository>();
        _mockCacheService = new Mock<ICacheService>();
        _mockMapper = new Mock<IMapper>();
        _service = new FoodItemService(_mockRepository.Object, _mockCacheService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var foodItems = new List<FoodItem>
        {
            new() { Id = Guid.NewGuid(), Name = "苹果", Quantity = 5, Unit = "个" },
            new() { Id = Guid.NewGuid(), Name = "牛奶", Quantity = 1, Unit = "瓶" }
        };

        var expectedDtos = new List<FoodItemDto>
        {
            new() { Id = foodItems[0].Id, Name = "苹果", Quantity = 5, Unit = "个" },
            new() { Id = foodItems[1].Id, Name = "牛奶", Quantity = 1, Unit = "瓶" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(foodItems);
        _mockMapper.Setup(m => m.Map<IEnumerable<FoodItemDto>>(foodItems)).Returns(expectedDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockMapper.Verify(m => m.Map<IEnumerable<FoodItemDto>>(foodItems), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddFoodItemAndClearCache()
    {
        // Arrange
        var request = new CreateFoodItemRequest
        {
            Name = "苹果",
            Quantity = 5,
            Unit = "个",
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        var foodItem = new FoodItem
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Quantity = request.Quantity,
            Unit = request.Unit,
            AddedDate = request.AddedDate,
            ExpiryDate = request.ExpiryDate,
            Status = FoodStatus.Normal
        };

        var expectedDto = new FoodItemDto
        {
            Id = foodItem.Id,
            Name = foodItem.Name,
            Quantity = foodItem.Quantity,
            Unit = foodItem.Unit,
            Status = foodItem.Status
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<FoodItem>())).Returns(Task.CompletedTask);
        _mockCacheService.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<FoodItemDto>(It.IsAny<FoodItem>())).Returns(expectedDto);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<FoodItem>()), Times.Once);
        _mockCacheService.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Exactly(2));
    }
} 