using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Errores;

namespace PetFamily.Application.PetsManagement.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<Result<PetDto, ErrorList>, GetPetByIdQuery>
{
	private readonly IReadDbContext _db;

	public GetPetByIdHandler(IReadDbContext readDbContext)
	{
		_db = readDbContext;
	}

	public async Task<Result<PetDto, ErrorList>> HandleAsync(
		GetPetByIdQuery query,
		CancellationToken token)
	{
		var pet = await _db.Pets
			.FirstOrDefaultAsync(v => v.Id == query.Id, token);

		if (pet is null)
			return Errors.General.NotFound(query.Id).ToErrorList();

		return pet;
	}
}
