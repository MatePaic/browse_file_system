using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), opt => opt.EnableRetryOnFailure());
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
