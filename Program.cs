using SparkApi.Data;
using SparkApi.Services;
using SparkApi.Utils;
using DotNetEnv;
using Serilog;
using SparkApi.Repositories;
using Snowflake.Data.Client;
using SparkApi.Repositories.Interfaces;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.SerilogConfiguration();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev",
    builder =>
    {
        builder.WithOrigins("http://localhost:5173", "http://localhost:7184")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Dapper
builder.Services.AddSingleton<ApiDbContext>();
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;


//builder.Services.AddTransient<DbService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<SnowflakeRepository>();
builder.Services.AddScoped<SnowflakeService>();



builder.Services.AddSingleton(provider =>
{
    var connectionString = Environment.GetEnvironmentVariable("SnowflakeConnection");
    var conn = new SnowflakeDbConnection
    {
        ConnectionString = connectionString
    };
    return conn;
});


//builder.Services.AddHostedService<StartupService>();

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

app.UseSerilogRequestLogging();
app.UseCors("Dev");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
