using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Specieses.Domain.IDs;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Specieses.Application.Commands.Delete;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
	private readonly ISpeciesRepository speciesRepository;
	private readonly IValidator<DeleteSpeciesCommand> validator;
	private readonly ILogger<DeleteSpeciesHandler> logger;
	private readonly IVolunteersContract volunteersContract;
	private readonly IReadDbContext readDbContext;

	public DeleteSpeciesHandler(
		ISpeciesRepository speciesRepository,
		IValidator<DeleteSpeciesCommand> validator,
		ILogger<DeleteSpeciesHandler> logger,
		IVolunteersContract volunteersContract,
		IReadDbContext readDbContext)
	{
		this.speciesRepository = speciesRepository;
		this.validator = validator;
		this.logger = logger;
		this.volunteersContract = volunteersContract;
		this.readDbContext = readDbContext;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(DeleteSpeciesCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var isAnySpecies = await volunteersContract.IsAnySpeciesFromPetAsync(command.SpeciesId, token);

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
