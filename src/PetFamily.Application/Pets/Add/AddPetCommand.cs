using PetFamily.Contracts.DTOs;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.Pets.Add;

public record AddPetCommand(
	Guid VolunteerId,
	string Name,
	PetTypes Type,
	string Description,
	Guid BreedId,
	Guid SpeciesId,
	string Color,
	decimal Weight,
	decimal Height,
	string Phone,
	PetHelpStatuses HelpStatus,
	AddressDTO Address
	);
