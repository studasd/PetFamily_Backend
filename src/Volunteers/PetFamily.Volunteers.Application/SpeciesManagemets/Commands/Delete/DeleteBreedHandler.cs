using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.SpeciesManagement.IDs;

namespace PetFamily.Volunteers.Application.SpeciesManagemets.Commands.Delete;

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
	private readonly ISpeciesRepository speciesRepository;
	private readonly IValidator<DeleteBreedCommand> validator;
	private readonly ILogger<DeleteBreedHandler> logger;
	private readonly IReadDbContext readDbContext;

	public DeleteBreedHandler(
		ISpeciesRepository speciesRepository,
		IValidator<DeleteBreedCommand> validator,
		ILogger<DeleteBreedHandler> logger,
		IReadDbContext readDbContext)
	{
		this.speciesRepository = speciesRepository;
		this.validator = validator;
		this.logger = logger;
		this.readDbContext = readDbContext;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(DeleteBreedCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var isAnyBreed = await readDbContext.Pets
				.AnyAsync(p => p.BreedId == command.BreedId, token);

		if (isAnyBreed)
			return Errors.General.AlreadyIsUsed("Breed").ToErrorList();


		var speciesResult = await speciesRepository.GetByIdAsync(SpeciesId.Create(command.SpeciesId), token);

		if (speciesResult.IsFailure)
			return speciesResult.Error.ToErrorList();

		var deleteBreedResult = speciesResult.Value.DeleteBreed(command.BreedId);
		if (deleteBreedResult.IsFailure)
			return deleteBreedResult.Error.ToErrorList();

		await speciesRepository.SaveAsync(token);

		logger.LogInformation("Deleted breed with id {breedId}", command.BreedId);

		return command.BreedId;
	}
}
