using Dapper;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Models;
using System.Text.Json;

namespace PetFamily.Volunteers.Application.PetsManagement.Queries.GetPetsWithPagination;

public class GetFilteredPetsWithPaginationDapper : IQueryHandler<PageList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
	private readonly ISqlConnectFactory sqlConnectFactory;

	public GetFilteredPetsWithPaginationDapper(ISqlConnectFactory sqlConnectFactory)
	{
		this.sqlConnectFactory = sqlConnectFactory;
	}

	public async Task<PageList<PetDto>> HandleAsync(
		GetFilteredPetsWithPaginationQuery query,
		CancellationToken token)
	{
		var db = sqlConnectFactory.Create();

		var totalCount = await db.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM pets");

		var sql = """
				SELECT id, name, color, file_storages FROM pets
				ORDER BY position LIMIT @PageSize OFFSET @Offset 
			""";
		var parameters = new 
		{ 
			query.PageSize, 
			Offset = (query.Page - 1) * query.PageSize 
		};

		var pets = await db.QueryAsync<PetDto, string, PetDto>(
			sql, 
			(pet, filesJson) => 
			{
				var files = JsonSerializer.Deserialize<FileStorageDto[]>(filesJson);
				pet.FileStorages = files;
				return pet;
			},
			parameters,
			splitOn: "file_storages"
		);

		return new PageList<PetDto>
		{
			Items = pets.ToList(), 
			Page = query.Page, 
			PageSize = query.PageSize,
			TotalCount = totalCount
		};
	}
}