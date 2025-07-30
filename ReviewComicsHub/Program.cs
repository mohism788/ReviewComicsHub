using ComicsAPI.DataAccess;
using ComicsAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IComicsRepository, ComicsRepository>();
builder.Services.AddScoped<IIssuesApiRepository, IssuesApiRepository>();
builder.Services.AddScoped<IReviewApiRepository, ReviewApiRepository>();
//addscoped Ilogger
builder.Services.AddScoped<ILogger, Logger<ComicsRepository>>();
builder.Services.AddScoped<ILogger, Logger<IssuesApiRepository>>();
builder.Services.AddScoped<ILogger, Logger<IReviewApiRepository>>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
