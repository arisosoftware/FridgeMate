using Microsoft.AspNetCore.StaticFiles;
using HandlebarsDotNet;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 配置静态文件
builder.Services.AddDirectoryBrowser();

var app = builder.Build();

// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 配置静态文件服务
app.UseStaticFiles();
app.UseDirectoryBrowser();

// 配置路由
app.MapGet("/", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/home.hbs");
    var compiledTemplate = Handlebars.Compile(template);
    
    // 模拟数据
    var data = new
    {
        totalFoodItems = 25,
        expiringFoodItems = 3,
        expiredFoodItems = 1,
        totalRecipes = 12,
        reminders = new[]
        {
            new { id = "1", name = "苹果", quantity = 5, unit = "个", expiryDate = "2024-02-01", isExpired = false, isExpiring = true },
            new { id = "2", name = "牛奶", quantity = 1, unit = "瓶", expiryDate = "2024-01-30", isExpired = true, isExpiring = false }
        },
        recommendedRecipes = new[]
        {
            new { id = "1", name = "红烧肉", description = "经典家常菜", cookingTime = 45, difficulty = "中等", imageUrl = "/images/recipe1.jpg" },
            new { id = "2", name = "宫保鸡丁", description = "川菜代表", cookingTime = 30, difficulty = "简单", imageUrl = "/images/recipe2.jpg" }
        }
    };
    
    var result = compiledTemplate(data);
    
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.MapGet("/foods", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/foods/list.hbs");
    var compiledTemplate = Handlebars.Compile(template);
    
    // 模拟数据
    var data = new
    {
        foodItems = new[]
        {
            new { id = "1", name = "苹果", quantity = 5, unit = "个", addedDate = "2024-01-25", expiryDate = "2024-02-01", status = "normal", isExpired = false, isExpiring = true, notes = "红富士苹果", expiryProgress = 70, daysUntilExpiry = 3 },
            new { id = "2", name = "牛奶", quantity = 1, unit = "瓶", addedDate = "2024-01-20", expiryDate = "2024-01-30", status = "expired", isExpired = true, isExpiring = false, notes = "全脂牛奶", expiryProgress = 100, daysUntilExpiry = -1 }
        },
        pagination = new { currentPage = 1, totalPages = 1, hasPrevious = false, hasNext = false }
    };
    
    var result = compiledTemplate(data);
    
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.MapGet("/foods/edit", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/foods/edit.hbs");
    var compiledTemplate = Handlebars.Compile(template);
    
    // 模拟数据
    var data = new
    {
        foodItem = new { id = "", name = "", quantity = 0, unit = "", addedDate = "", expiryDate = "", notes = "" }
    };
    
    var result = compiledTemplate(data);
    
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.MapGet("/recipes", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/recipes/list.hbs");
    var compiledTemplate = Handlebars.Compile(template);
    
    // 模拟数据
    var data = new
    {
        recipes = new[]
        {
            new { 
                id = "1", 
                name = "红烧肉", 
                description = "经典家常菜，肥而不腻", 
                cookingTime = 45, 
                difficulty = "medium", 
                difficultyText = "中等",
                servings = 4,
                ingredientCount = 8,
                category = "chinese",
                categoryText = "中餐",
                imageUrl = "/images/recipe1.jpg",
                isCookable = true,
                isPartiallyCookable = false,
                cookableStatus = "cookable",
                availablePercentage = 100,
                missingIngredients = "",
                tags = new[] { "家常菜", "下饭菜" }
            },
            new { 
                id = "2", 
                name = "宫保鸡丁", 
                description = "川菜代表，麻辣鲜香", 
                cookingTime = 30, 
                difficulty = "easy", 
                difficultyText = "简单",
                servings = 2,
                ingredientCount = 6,
                category = "chinese",
                categoryText = "中餐",
                imageUrl = "/images/recipe2.jpg",
                isCookable = false,
                isPartiallyCookable = true,
                cookableStatus = "partial",
                availablePercentage = 60,
                missingIngredients = "花生米,干辣椒",
                tags = new[] { "川菜", "辣" }
            }
        },
        pagination = new { currentPage = 1, totalPages = 1, hasPrevious = false, hasNext = false }
    };
    
    var result = compiledTemplate(data);
    
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.MapGet("/recipes/detail/{id}", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/recipes/detail.hbs");
    var compiledTemplate = Handlebars.Compile(template);
    
    // 模拟数据
    var data = new
    {
        recipe = new
        {
            id = "1",
            name = "红烧肉",
            description = "经典家常菜，肥而不腻，入口即化",
            cookingTime = 45,
            difficulty = "medium",
            difficultyText = "中等",
            servings = 4,
            ingredientCount = 8,
            category = "chinese",
            categoryText = "中餐",
            imageUrl = "/images/recipe1.jpg",
            isCookable = true,
            isPartiallyCookable = false,
            tags = new[] { "家常菜", "下饭菜" },
            ingredients = new[]
            {
                new { id = "1", name = "五花肉", quantity = 500, unit = "克", isAvailable = true, isPartiallyAvailable = false, missingQuantity = 0 },
                new { id = "2", name = "生抽", quantity = 30, unit = "毫升", isAvailable = true, isPartiallyAvailable = false, missingQuantity = 0 },
                new { id = "3", name = "老抽", quantity = 15, unit = "毫升", isAvailable = false, isPartiallyAvailable = false, missingQuantity = 15 }
            },
            instructions = new[]
            {
                "五花肉切成大块，冷水下锅焯水",
                "锅中放油，放入冰糖炒至焦糖色",
                "放入五花肉翻炒上色",
                "加入生抽、老抽、料酒调味",
                "加入适量清水，大火烧开后转小火炖煮45分钟"
            },
            nutritionInfo = new
            {
                calories = 350,
                protein = 25,
                fat = 28,
                carbohydrates = 5
            },
            averageRating = new[] { true, true, true, true, false },
            averageRatingValue = 4.2,
            reviewCount = 15,
            availabilityStats = new
            {
                availableCount = 6,
                partialCount = 1,
                unavailableCount = 1,
                availablePercentage = 75,
                partialPercentage = 12.5,
                unavailablePercentage = 12.5
            }
        }
    };
    
    var result = compiledTemplate(data);
    
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.MapGet("/recipes/create", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/recipes/create.hbs");
    var compiledTemplate = Handlebars.Compile(template);
    
    // 模拟数据
    var data = new
    {
        ingredients = new object[] { },
        instructions = new object[] { }
    };
    
    var result = compiledTemplate(data);
    
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.Run(); 