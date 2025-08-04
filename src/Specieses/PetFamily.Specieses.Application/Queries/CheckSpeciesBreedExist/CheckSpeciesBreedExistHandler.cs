using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Domain.IDs;

namespace PetFamily.Specieses.Application.Queries.CheckSpeciesBreedExist;

public class CheckSpeciesBreedExistHandler : IQueryHandler<Result<bool, ErrorList>, CheckSpeciesBreedExistQuery>
{
	private readonly IReadDbContext db;

	public CheckSpeciesBreedExistHandler(IReadDbContext readDbContext)
	{
		db = readDbContext;
	}

	public async Task<Result<bool, ErrorList>> HandleAsync(CheckSpeciesBreedExistQuery query, CancellationToken token)
	{
		var isSpeciesExist = await db.Species
				.AnyAsync(b => b.Id == query.SpeciesId, token);
		if (!isSpeciesExist)
			return Errors.General.NotFound(query.SpeciesId).ToErrorList();

		var isBreedExist = await db.Breeds
				.AnyAsync(b => b.Id == query.BreedId, token);
		if (!isBreedExist)
			return Errors.General.NotFound(query.BreedId).ToErrorList();

		return true;
	}
}