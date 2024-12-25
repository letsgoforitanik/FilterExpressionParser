namespace FilterExpressionParser.Models;

public class Movie
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required int YearOfRelease { get; init; }
    public required string Slug { get; init; }
    public required IEnumerable<string> Genres { get; init; }
}