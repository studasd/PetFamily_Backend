using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Errores;
using PetFamily.Domain.SpeciesManagement.IDs;

namespace PetFamily.Application.SpeciesManagemets.Commands.Delete;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
	private readonly ISpeciesRepository speciesRepository;
	private readonly IValidator<DeleteSpeciesCommand> validator;
	private readonly ILogger<DeleteSpeciesHandler> logger;
	private readonly IReadDbContext readDbContext;

	public DeleteSpeciesHandler(
		ISpeciesRepository speciesRepository,
		IValidator<DeleteSpeciesCommand> validator,
		ILogger<DeleteSpeciesHandler> logger,
		IReadDbContext readDbContext)
	{
		this.speciesRepository = speciesRepository;
		this.validator = validator;
		this.logger = logger;
		this.readDbContext = readDbContext;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(DeleteSpeciesCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var isAnySpecies = await readDbContext.Pets
				.AnyAsync(p => p.SpeciesId == command.SpeciesId, token);

		if (isAnySpecies)
			return Errors.General.AlreadyIsUsed("Species").ToErrorList();


		var speciesResult = await speciesRepository.GetByIdAsync(SpeciesId.Create(command.SpeciesId), token);

		if (speciesResult.IsFailure)
			return speciesResult.Error.ToErrorList();

		await speciesRepository.DeleteAsync(speciesResult.Value, token);

		logger.LogInformation("Deleted species with id {speciesId}", speciesResult.Value.Id);

		return command.SpeciesId;
	}
}
