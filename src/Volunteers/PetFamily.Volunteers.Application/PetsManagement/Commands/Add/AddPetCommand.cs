using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Enums;
using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.Add;

public record AddPetCommand(
	Guid VolunteerId,
	string Name,
	string Description,
	Guid BreedId,
	Guid SpeciesId,
	string Color,
	decimal Weight,
	decimal Height,
	string Phone,
	PetHelpStatuses HelpStatus,
	AddressDTO Address
	) : ICommand;
