using PetFamily.Contracts.DTOs;
using PetFamily.Contracts.Enums;

namespace PetFamily.Contracts.RequestPets;

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
