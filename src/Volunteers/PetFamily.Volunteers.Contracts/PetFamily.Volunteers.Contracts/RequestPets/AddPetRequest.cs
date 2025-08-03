using PetFamily.Volunteers.Contracts.DTOs;
using PetFamily.Volunteers.Contracts.Enums;

namespace PetFamily.Volunteers.Contracts.RequestPets;

public record AddPetRequest(
	string Name,
	string Description,
	Guid BreedId,
	Guid SpeciesId,
	string Color,
	decimal Weight,
	decimal Height,
	string Phone,
	PetHelpStatuses HelpStatus,
	AddressDTO AddressDTO);
