using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Core.Abstractions;
using System.Linq.Expressions;

namespace PetFamily.Application.PetsManagement.Queries.GetPetsWithPagination;

public class GetFilteredPetsWithPaginationHandler : IQueryHandler<PageList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
	private readonly IReadDbContext _db;

	public GetFilteredPetsWithPaginationHandler(IReadDbContext readDbContext)
	{
		_db = readDbContext;
	}

	public async Task<PageList<PetDto>> HandleAsync(
		GetFilteredPetsWithPaginationQuery query, 
		CancellationToken token)
	{
		var petQuery = _db.Pets;

		petQuery = petQuery.WhereIf(
			query.VolunteerIds is not null && query.VolunteerIds.Any(),
			x => query.VolunteerIds!.Contains(x.VolunteerId));

		petQuery = petQuery.WhereIf(
			!String.IsNullOrWhiteSpace(query.Name), 
			x => x.Name.Contains(query.Name!));

		petQuery = petQuery.WhereIf(
			query.Age is not null,
			x => DateTime.UtcNow.Year - x.DateBirth.Year < query.Age);

		petQuery = petQuery.WhereIf(
			query.SpeciesId is not null,
			x => x.SpeciesId == query.SpeciesId);

		petQuery = petQuery.WhereIf(
			query.BreedId is not null,
			x => x.BreedId == query.BreedId);

		petQuery = petQuery.WhereIf(
			!String.IsNullOrWhiteSpace(query.Color),
			x => x.Color == query.Color);

		petQuery = petQuery.WhereIf(
			query.Weight is not null,
			x => x.Weight <= query.Weight);

		petQuery = petQuery.WhereIf(
			query.Height is not null,
			x => x.Height <= query.Height);

		petQuery = petQuery.WhereIf(
			!String.IsNullOrWhiteSpace(query.Country),
			x => x.AddressCountry == query.Country);

		petQuery = petQuery.WhereIf(
			!String.IsNullOrWhiteSpace(query.City),
			x => x.AddressCity == query.City);

		petQuery = petQuery.WhereIf(
			query.HelpStatus is not null,
			x => x.HelpStatus == query.HelpStatus);

		petQuery = petQuery.WhereIf(
			query.PositionFrom is not null, 
			x => x.Position >= query.PositionFrom!.Value);

		petQuery = petQuery.WhereIf(
			query.PositionTo is not null, 
			x => x.Position <= query.PositionTo!.Value);


		Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
		{
			"volunteerid" => (pet) => pet.VolunteerId,
			"name" => (pet) => pet.Name,
			"age" => (pet) => pet.DateBirth.Year,
			"speciesid" => (pet) => pet.SpeciesId,
			"breedid" => (pet) => pet.BreedId,
			"color" => (pet) => pet.Color,
			"weight" => (pet) => pet.Weight,
			"height" => (pet) => pet.Height,
			"country" => (pet) => pet.AddressCountry,
			"city" => (pet) => pet.AddressCity,
			"helpstatus" => (pet) => pet.HelpStatus,
			"position" => (pet) => pet.Position,
			_ => (pet) => pet.Id
		};

		petQuery = query.SortDirection?.ToLower() == "desc"
			? petQuery.OrderByDescending(keySelector)
			: petQuery.OrderBy(keySelector);


		var pets = await petQuery
			.ToPagedListAsync(query.Page, query.PageSize, token);

		return pets;
	}
}
