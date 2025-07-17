using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Models;

namespace PetFamily.Application.Extensions;

public static class QueriesExtensions
{
	public static async Task<PageList<T>> ToPagedListAsync<T>(
		this IQueryable<T> source,
		int page,
		int pageSize,
		CancellationToken token)
	{
		var totalCount = await source.CountAsync(token);

		var items = await source
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(token);

		return new PageList<T>
		{
			Pets = items,
			PageSize = pageSize,
			Page = page,
			TotalCount = totalCount
		};
	}
}