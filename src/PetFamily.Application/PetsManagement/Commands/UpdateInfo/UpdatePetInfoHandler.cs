using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteerManagement;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Errores;
using PetFamily.Core.ValueObjects;
using PetFamily.Domain.Entities;
using PetFamily.Domain.SpeciesManagement.IDs;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.PetsManagement.Commands.UpdateInfo;

public class UpdatePetInfoHandler : ICommandHandler<Guid, UpdatePetInfoCommand>
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<UpdatePetInfoCommand> validator;
	private readonly IReadDbContext db;
	private readonly ILogger<UpdatePetInfoHandler> logger;

	public UpdatePetInfoHandler(
		IVolunteerRepository volunteerRepository,
		IValidator<UpdatePetInfoCommand> validator,
		IReadDbContext readDbContext,
		ILogger<UpdatePetInfoHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.db = readDbContext;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UpdatePetInfoCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var isSpeciesExist = await db.Species
				.AnyAsync(b => b.Id == command.SpeciesId, token);
		if (!isSpeciesExist)
			return Errors.General.NotFound(command.SpeciesId).ToErrorList();

		var isBreedExist = await db.Breeds
				.AnyAsync(b => b.Id == command.BreedId, token);
		if (!isBreedExist)
			return Errors.General.NotFound(command.BreedId).ToErrorList();

		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);
		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		var petResult = volunteerResult.Value.GetPetById(command.PetId);
		if (petResult.IsFailure)
			return petResult.Error.ToErrorList();

		var petId = PetId.Create(command.PetId);
		var speciesId = SpeciesId.Create(command.SpeciesId);
		var petType = PetType.Create(command.SpeciesId, command.BreedId).Value;

		var addressDto = command.Address;
		var address = Address.Create(
			addressDto.Country, 
			addressDto.City, 
			addressDto.Street, 
			addressDto.HouseNumber, 
			addressDto.Apartment, 
			addressDto.HouseLiter).Value;

		var phones = command.Phones.Select(p => Phone.Create(p).Value);
		var bankingВetails = BankingDetails.Create(command.BankingВetails.Name, command.BankingВetails.Description).Value;

		var petInfo = new PetUpdateInfoDto(
			command.Name,
			command.Description,
			command.Color,
			command.HealthInfo,
			address,
			command.Weight,
			command.Height,
			phones,
			command.IsNeutered,
			command.IsVaccinated,
			command.DateBirth,
			command.HelpStatus,
			bankingВetails,
			petType
		);

		var updateResult = volunteerResult.Value.UpdatePetInfo(petId, petInfo);
		if (updateResult.IsFailure)
			return updateResult.Error.ToErrorList();

		logger.LogInformation(
				"Pet information with ID {petId} updated by volunteer with ID {volunteerId}",
				command.PetId,
				command.VolunteerId
			);

		await volunteerRepository.SaveAsync(token);

		return command.PetId;
	}
}
