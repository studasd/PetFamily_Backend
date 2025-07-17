namespace PetFamily.Application.Models;

public class PageList<T>
{
	public IReadOnlyList<T> Pets { get; init; }

	public int TotalCount { get; init; }

	public int PageSize {  get; init; }

	public int Page {  get; init; }

	public bool HasNextPage => Page * PageSize < TotalCount;

	public bool HasPreviousPage => Page > 1;
}