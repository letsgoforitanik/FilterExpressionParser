using FilterExpressionParser.Data;
using FilterExpressionParser.Dto;
using FilterExpressionParser.Extensions;
using FilterExpressionParser.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilterExpressionParser.Controllers;

[Route("api/movies")]
[ApiController]
public class MovieController(MovieDbContext db) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetMoviesAsync([FromQuery] Filter<MovieFilterAttributes> filterOptions)
    {
        var results = await db.Movies.Filter(filterOptions).ToListAsync();
        return Ok(results);
    }
}