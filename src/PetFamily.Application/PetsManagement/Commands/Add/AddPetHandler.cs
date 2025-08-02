using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesManagemets;
using PetFamily.Application.VolunteerManagement;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Errores;
using PetFamily.Core.ValueObjects;
using PetFamily.Domain.SpeciesManagement.IDs;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.PetsManagement.Commands.Add;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand> // CreatePetService
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ISpeciesRepository speciesRepository;
	private readonly IValidator<AddPetCommand> validator;
	private readonly IReadDbContext db;
	private readonly ILogger<AddPetHandler> logger;

	public AddPetHandler(
		IVolunteerRepository volunteerRepository, 
		ISpeciesRepository speciesRepository,
		IValidator<AddPetCommand> validator,
		IReadDbContext readDbContext,
		ILogger<AddPetHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.speciesRepository = speciesRepository;
		this.validator = validator;
		this.db = readDbContext;
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

		var isSpeciesExist = await db.Species
				.AnyAsync(b => b.Id == command.SpeciesId, token);
		if (!isSpeciesExist)
			return Errors.General.NotFound(command.SpeciesId).ToErrorList();

		var isBreedExist = await db.Breeds
				.AnyAsync(b => b.Id == command.BreedId, token);
		if (!isBreedExist)
			return Errors.General.NotFound(command.BreedId).ToErrorList();


		var petTypeResult = PetType.Create(BreedId.Create(command.BreedId), SpeciesId.Create(command.SpeciesId));
		if (petTypeResult.IsFailure)
			return petTypeResult.Error.ToErrorList();

		var pet = Pet.Create(
			command.Name,
			command.Description,
			command.Color,
			command.Weight,
			command.Height,
			phone,
			command.HelpStatus,
			address,
			petTypeResult.Value).Value;

		volunteer.Value.AddPet(pet);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Created pet {petName} with id {petId}", pet.Name, pet.Id);

		return pet.Id.Value;
	}
}
