namespace AlthiraProducts.BuildingBlocks.Application.Models.Pagination;

public sealed class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = null!;
    public long TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}