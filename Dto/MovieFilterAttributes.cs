namespace FilterExpressionParser.Dto;

public abstract class MovieFilterAttributes
{
    public required string Title { get; init; }
    public required int YearOfRelease { get; init; }
}