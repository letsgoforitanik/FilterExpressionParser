using FilterExpressionParser.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<MovieDbContext>(opt => opt.UseSqlServer(builder.Configuration["DbConnectionString"]));

var app = builder.Build();
app.MapControllers();
app.Run();
