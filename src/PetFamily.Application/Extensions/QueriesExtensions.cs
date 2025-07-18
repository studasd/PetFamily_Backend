using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Models;
using System.Linq.Expressions;

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
			Items = items,
			PageSize = pageSize,
			Page = page,
			TotalCount = totalCount
		};
	}


	public static IQueryable<T> WhereIf<T>(
		this IQueryable<T> source,
		bool condition,
		Expression<Func<T, bool>> predicate)
	{
		return condition ? source.Where(predicate) : source;
	}
}