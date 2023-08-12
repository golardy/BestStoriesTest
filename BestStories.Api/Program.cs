using BestStories.Api;
using Microsoft.OpenApi.Models;
using Serilog.Formatting.Json;
using Serilog;
using BestStories.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.AddJsonFile("appsettings.json", false).Build();
builder.Services.AddHttpClient("BestStoriesClient", c => 
{
    c.BaseAddress = new Uri(configuration.GetSection(HackerNewsApiConfig.Section).Get<HackerNewsApiConfig>().BaseUri);
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.RegisterServices();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Best stories retriver service", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

Log.Logger = new LoggerConfiguration()
.WriteTo.Console(new JsonFormatter())
.CreateLogger();

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}