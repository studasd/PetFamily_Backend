using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Contracts.Requests;
using PetFamily.Volunteers.Application.VolunteerManagement;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.UpdateInfo;

public class UpdatePetInfoHandler : ICommandHandler<Guid, UpdatePetInfoCommand>
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<UpdatePetInfoCommand> validator;
	private readonly IReadDbContext db;
	private readonly ISpeciesContract speciesContract;
	private readonly ILogger<UpdatePetInfoHandler> logger;

	public UpdatePetInfoHandler(
		IVolunteerRepository volunteerRepository,
		IValidator<UpdatePetInfoCommand> validator,
		IReadDbContext readDbContext,
		ISpeciesContract speciesContract,
		ILogger<UpdatePetInfoHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		db = readDbContext;
		this.speciesContract = speciesContract;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UpdatePetInfoCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var request = new CheckSpeciesBreedExistRequest(command.SpeciesId, command.BreedId);
		var isSpeciesBreedExistResult = speciesContract.CheckSpeciesBreedExistAsync(request, token);
		if (isSpeciesBreedExistResult.IsFaulted)
			return isSpeciesBreedExistResult.Result.Error;

		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);
		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		var petResult = volunteerResult.Value.GetPetById(command.PetId);
		if (petResult.IsFailure)
			return petResult.Error.ToErrorList();

		var petType = new PetType(command.SpeciesId, command.BreedId);

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

		var updateResult = volunteerResult.Value.UpdatePetInfo(command.PetId, petInfo);
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
