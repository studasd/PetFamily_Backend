using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Contracts.Requests;
using PetFamily.Volunteers.Application.VolunteerManagement;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.Add;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand> // CreatePetService
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ISpeciesContract speciesContract;
	private readonly IValidator<AddPetCommand> validator;
	private readonly IReadDbContext db;
	private readonly ILogger<AddPetHandler> logger;

	public AddPetHandler(
		IVolunteerRepository volunteerRepository,
		ISpeciesContract speciesContract,
		IValidator<AddPetCommand> validator,
		IReadDbContext readDbContext,
		ILogger<AddPetHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.speciesContract = speciesContract;
		this.validator = validator;
		db = readDbContext;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(AddPetCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var volunteer = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);

		var addressDto = command.Address;

		var phone = Phone.Create(command.Phone).Value;
		var address = Address.Create(addressDto.Country, addressDto.City, addressDto.Street, addressDto.HouseNumber, addressDto.Apartment, addressDto.HouseLiter).Value;

		var request = new CheckSpeciesBreedExistRequest(command.SpeciesId, command.BreedId);
		var checkSpeciesBreedExistResult = await speciesContract.CheckSpeciesBreedExistAsync(request, token);
		if (checkSpeciesBreedExistResult.IsFailure)
			return checkSpeciesBreedExistResult.Error;

		var petType = new PetType(command.BreedId, command.SpeciesId);

		var pet = Pet.Create(
			command.Name,
			command.Description,
			command.Color,
			command.Weight,
			command.Height,
			phone,
			command.HelpStatus,
			address,
			petType).Value;

		volunteer.Value.AddPet(pet);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Created pet {petName} with id {petId}", pet.Name, pet.Id);

		return pet.Id.Value;
	}
}
