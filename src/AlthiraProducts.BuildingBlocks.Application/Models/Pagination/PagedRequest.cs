namespace AlthiraProducts.BuildingBlocks.Application.Models.Pagination;

public sealed class PagedRequest
{
    private const int MaxPageSize = 50;

    public int Page { get; init; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaxPageSize 
            ? MaxPageSize 
            : value;
    }

    public string? OrderBy { get; init; }
    public bool Descending { get; init; }
}