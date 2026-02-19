using Serilog;
using TaskManagerAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/taskmanager-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)         
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureMediatR();
builder.Services.ConfigureValidators();
builder.Services.ConfigureAutoMapper();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.Configure<Domain.Configuration.JwtConfiguration>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.ConfigureJWT(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

await app.SeedDataAsync();

app.Run();
