var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();


app.MapPost("/generate-docx", async (
    WordTemplateService wordService,
    IWebHostEnvironment env
) =>
{
    var model = new {
        Name = "Alice",
        ReportDate = DateTime.Now.ToShortDateString()
    };

    var baseDir = Path.Combine(env.ContentRootPath, "Templates");
    var outputPath = Path.Combine(baseDir, "output.docx");

    wordService.RenderTemplate($"{baseDir}/main.docx", model, outputPath);
    wordService.InsertSubDocAtBookmark(outputPath, $"{baseDir}/section.docx", "SectionPlaceholder");

    return Results.File(outputPath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "output.docx");
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
