using FilterExpressionParser.Models;
using Microsoft.EntityFrameworkCore;

namespace FilterExpressionParser.Data;

public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
{
    public required DbSet<Movie> Movies { get; init; }
}