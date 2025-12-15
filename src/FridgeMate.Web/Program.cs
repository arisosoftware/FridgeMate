using System.Net.Http.Json;
using HandlebarsDotNet;

var builder = WebApplication.CreateBuilder(args);

// API 基础地址配置（优先级：配置文件 -> 环境变量 -> 默认值）
var apiBaseAddress = builder.Configuration["ApiBaseUrl"]
                     ?? Environment.GetEnvironmentVariable("FRIDGEMATE_API_BASE_URL")
                     ?? "http://localhost:5001/api/";

builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

// 添加服务到容器
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDirectoryBrowser();

RegisterHandlebarsHelpers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseDirectoryBrowser();

// 首页暂时保留示例数据（后续可接入 API）
app.MapGet("/", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/home.hbs");
    var compiledTemplate = Handlebars.Compile(template);

    var result = compiledTemplate(new
    {
        totalFoodItems = 0,
        expiringFoodItems = 0,
        expiredFoodItems = 0,
        totalRecipes = 0,
        reminders = Array.Empty<object>(),
        recommendedRecipes = Array.Empty<object>()
    });

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

// 食材列表：从 FridgeMate.API 获取真实数据
app.MapGet("/foods", async (IHttpClientFactory httpClientFactory, HttpContext context) =>
{
    var http = httpClientFactory.CreateClient("api");

    // 允许通过查询字符串覆盖分页和筛选
    var pageNumber = int.TryParse(context.Request.Query["pageNumber"], out var pn) ? pn : 1;
    var pageSize = int.TryParse(context.Request.Query["pageSize"], out var ps) ? ps : 20;
    var name = context.Request.Query["name"].ToString();
    var status = context.Request.Query["status"].ToString();

    var query = new List<string>
    {
        $"pageNumber={pageNumber}",
        $"pageSize={pageSize}"
    };
    if (!string.IsNullOrWhiteSpace(name)) query.Add($"name={Uri.EscapeDataString(name)}");
    if (!string.IsNullOrWhiteSpace(status)) query.Add($"status={Uri.EscapeDataString(status)}");

    var apiUrl = $"FoodItems/paged?{string.Join("&", query)}";

    PagedResult<FoodItemDto>? apiResult = null;
    try
    {
        apiResult = await http.GetFromJsonAsync<PagedResult<FoodItemDto>>(apiUrl);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[foods] 调用 API 失败: {ex.Message}");
    }

    var now = DateTime.Now;
    var foodItems = apiResult?.Items.Select(item =>
    {
        var statusText = item.Status?.ToLowerInvariant() switch
        {
            "expired" => "expired",
            "nearlyexpired" => "expiring",
            _ => "normal"
        };

        var isExpired = item.ExpiryDate <= now;
        var isExpiring = !isExpired && (item.ExpiryDate - now).TotalHours <= 48;

        var totalSpan = (item.ExpiryDate - item.AddedDate).TotalSeconds;
        var passedSpan = (now - item.AddedDate).TotalSeconds;
        var progress = totalSpan <= 0 ? 100 : (int)Math.Clamp(passedSpan / totalSpan * 100, 0, 100);

        var daysUntilExpiry = (int)Math.Floor((item.ExpiryDate - now).TotalDays);

        return new
        {
            id = item.Id,
            name = item.Name,
            quantity = item.Quantity,
            unit = item.Unit,
            addedDate = item.AddedDate,
            expiryDate = item.ExpiryDate,
            status = statusText,
            isExpired,
            isExpiring,
            notes = item.Notes ?? string.Empty,
            expiryProgress = progress,
            daysUntilExpiry
        };
    }).ToList() ?? new List<object>();

    var pagination = new
    {
        currentPage = apiResult?.PageNumber ?? 1,
        totalPages = apiResult?.TotalPages ?? 1,
        hasPrevious = (apiResult?.PageNumber ?? 1) > 1,
        hasNext = (apiResult?.PageNumber ?? 1) < (apiResult?.TotalPages ?? 1),
        previousPage = Math.Max((apiResult?.PageNumber ?? 1) - 1, 1),
        nextPage = Math.Min((apiResult?.PageNumber ?? 1) + 1, apiResult?.TotalPages ?? 1)
    };

    var template = await File.ReadAllTextAsync("Views/Templates/foods/list.hbs");
    var compiledTemplate = Handlebars.Compile(template);

    var viewModel = new
    {
        foodItems,
        pagination
    };

    var result = compiledTemplate(viewModel);

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

// 其他页面仍保留示例数据（后续可逐步对接 API）
app.MapGet("/foods/edit", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/foods/edit.hbs");
    var compiledTemplate = Handlebars.Compile(template);

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

    var result = compiledTemplate(new
    {
        recipes = Array.Empty<object>(),
        pagination = new { currentPage = 1, totalPages = 1, hasPrevious = false, hasNext = false }
    });

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.MapGet("/recipes/detail/{id}", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/recipes/detail.hbs");
    var compiledTemplate = Handlebars.Compile(template);

    var result = compiledTemplate(new { recipe = new { name = "菜谱详情", ingredients = Array.Empty<object>() } });

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.MapGet("/recipes/create", async context =>
{
    var template = await File.ReadAllTextAsync("Views/Templates/recipes/create.hbs");
    var compiledTemplate = Handlebars.Compile(template);

    var result = compiledTemplate(new { ingredients = Array.Empty<object>(), instructions = Array.Empty<object>() });

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(result);
});

app.Run();

static void RegisterHandlebarsHelpers()
{
    Handlebars.RegisterHelper("formatDate", (output, context, arguments) =>
    {
        if (arguments.Length == 0 || arguments[0] is null)
        {
            output.WriteSafeString(string.Empty);
            return;
        }

        if (arguments[0] is DateTime dt)
        {
            output.WriteSafeString(dt.ToString("yyyy-MM-dd"));
            return;
        }

        if (DateTime.TryParse(arguments[0]?.ToString(), out var parsed))
        {
            output.WriteSafeString(parsed.ToString("yyyy-MM-dd"));
            return;
        }

        output.WriteSafeString(arguments[0]?.ToString() ?? string.Empty);
    });

    Handlebars.RegisterHelper("formatDateForInput", (output, context, arguments) =>
    {
        if (arguments.Length == 0 || arguments[0] is null)
        {
            output.WriteSafeString(string.Empty);
            return;
        }

        if (arguments[0] is DateTime dt)
        {
            output.WriteSafeString(dt.ToString("yyyy-MM-dd"));
            return;
        }

        if (DateTime.TryParse(arguments[0]?.ToString(), out var parsed))
        {
            output.WriteSafeString(parsed.ToString("yyyy-MM-dd"));
            return;
        }

        output.WriteSafeString(string.Empty);
    });

    Handlebars.RegisterHelper("eq", (writer, context, parameters) =>
    {
        if (parameters.Length < 2)
        {
            writer.WriteSafeString("false");
            return;
        }

        writer.WriteSafeString(object.Equals(parameters[0], parameters[1]));
    });

    Handlebars.RegisterHelper("add", (writer, context, parameters) =>
    {
        if (parameters.Length < 2) return;

        if (double.TryParse(parameters[0]?.ToString(), out var a) &&
            double.TryParse(parameters[1]?.ToString(), out var b))
        {
            writer.WriteSafeString(a + b);
        }
    });
}

internal record FoodItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public float Quantity { get; init; }
    public string Unit { get; init; } = string.Empty;
    public DateTime AddedDate { get; init; }
    public DateTime ExpiryDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
}

internal record PagedResult<T>
{
    public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}